using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication1;

internal sealed class CookieOidcAndTokenRefresher(
    IOptionsMonitor<OpenIdConnectOptions> oidcOptionsMonitor,
    ILogger<CookieOidcAndTokenRefresher> logger)
{
    private readonly OpenIdConnectProtocolValidator _openIdConnectProtocolValidator = new()
    {
        RequireNonce = false
    };

    public async Task ValidateOrRefreshTokenAsync(CookieValidatePrincipalContext validateContext, string oidcScheme)
    {
        // get the token expiration time
        var accessTokenExpiration = validateContext.Properties.GetTokenValue("expires_at");
        if (!DateTimeOffset.TryParse(accessTokenExpiration, out var accessTokenExpiresAt))
        {
            logger.LogWarning("Access token expiration date is missing or invalid.");
            return;
        }
        
        // compare the token expiration time with the current time and return if the token is still valid
        var oidcOptions = oidcOptionsMonitor.Get(oidcScheme);
        var tokenValidTime = oidcOptions.TimeProvider!.GetUtcNow();
        if (tokenValidTime < accessTokenExpiresAt)
        {
            return;
        }
        
        // get the oidc config to get the token endpoint to request for a new one
        var oidcConfiguration = await oidcOptions.ConfigurationManager!.GetConfigurationAsync(validateContext.HttpContext.RequestAborted);
        var tokenEndpoint = oidcConfiguration.TokenEndpoint ?? throw new InvalidOperationException("Token endpoint is missing in the OIDC configuration.");
        
        // get the refresh token from 
        using var refreshTokenResponse = await oidcOptions.Backchannel.PostAsync(tokenEndpoint,
            new FormUrlEncodedContent(new Dictionary<string, string?>()
            {
                ["grant_type"] = "refresh_token",
                ["client_id"] = oidcOptions.ClientId,
                ["client_secret"] = oidcOptions.ClientSecret,
                ["scope"] = string.Join(" ", oidcOptions.Scope),
                ["refresh_token"] = validateContext.Properties.GetTokenValue("refresh_token"),
            }));

        if (!refreshTokenResponse.IsSuccessStatusCode)
        {
            logger.LogInformation("Unable to get the refresh token");
            validateContext.RejectPrincipal();
            return;
        }
        
        // deserialize the response to get the new token
        var refreshDataJson = await refreshTokenResponse.Content.ReadAsStringAsync();
        var message = new OpenIdConnectMessage(refreshDataJson);

        // get the validation parameters and set the base configuration manager if it is available
        var validationParameters = oidcOptions.TokenValidationParameters.Clone();
        if (oidcOptions.ConfigurationManager is BaseConfigurationManager baseConfigurationManager)
        {
            validationParameters.ConfigurationManager = baseConfigurationManager;
        }
        else
        {
            validationParameters.ValidIssuer = oidcConfiguration.Issuer;
            validationParameters.IssuerSigningKeys = oidcConfiguration.SigningKeys;
        }
        
        // validate the token
        var validationResult = await oidcOptions.TokenHandler.ValidateTokenAsync(message.IdToken, validationParameters);

        if (!validationResult.IsValid)
        {
            validateContext.RejectPrincipal();
            return;
        }
        
        // convert the id token into jwt
        var validatedIdToken = JwtSecurityTokenConverter.Convert(validationResult.SecurityToken as JsonWebToken);
        validatedIdToken.Payload["nonce"] = null;
        
        _openIdConnectProtocolValidator.ValidateTokenResponse(new OpenIdConnectProtocolValidationContext
        {
            ProtocolMessage = message,
            ValidatedIdToken = validatedIdToken,
            ClientId = oidcOptions.ClientId
        });
        
        // replace the old claims principal in the context with the new one
        validateContext.ShouldRenew = true;
        validateContext.ReplacePrincipal(new ClaimsPrincipal(validationResult.ClaimsIdentity));
        
        var expiresIn = int.Parse(message.ExpiresIn, NumberStyles.Integer, CultureInfo.InvariantCulture);
        var expiresAt = tokenValidTime + TimeSpan.FromSeconds(expiresIn);
        
        // update the tokens in the context
        validateContext.Properties.StoreTokens([
            new AuthenticationToken { Name = "access_token", Value = message.AccessToken },
            new AuthenticationToken { Name = "id_token", Value = message.IdToken },
            new AuthenticationToken { Name = "token_type", Value = message.TokenType },
            new AuthenticationToken { Name = "expires_at", Value = expiresAt.ToString("o", CultureInfo.InvariantCulture) },
        ]);
        logger.LogInformation("Token has been refreshed");
    }
}