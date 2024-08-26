using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication1;

public static class AuthExtensions
{
    public static WebApplicationBuilder ConfigureAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<OpenIdConnectOptions>()
            .BindConfiguration(nameof(OpenIdConnectOptions))
            .ValidateDataAnnotations();
        
        builder.Services.AddAntiforgery(o =>
        {
            o.HeaderName = "XSRF-TOKEN";
            o.Cookie.HttpOnly = true;
            o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });
        
        builder.Services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = SameSiteMode.Strict;
            options.HttpOnly = HttpOnlyPolicy.Always;
            options.Secure = CookieSecurePolicy.Always;
        });
        
        builder.Services.AddAuthorization();
        
        var openIdConnectOptions = builder.Services.BuildServiceProvider()
            .GetRequiredService<IOptions<OpenIdConnectOptions>>().Value;

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = "spa_cookie";
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.SlidingExpiration = true;
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = openIdConnectOptions.Authority;

                options.ClientId = openIdConnectOptions.ClientId;
                options.ResponseType = openIdConnectOptions.ResponseType;
                options.ResponseMode = openIdConnectOptions.ResponseMode;

                options.Scope.Clear();

                foreach (var scope in openIdConnectOptions.Scope)
                {
                    options.Scope.Add(scope);
                }

                options.CallbackPath = openIdConnectOptions.CallbackPath;
                options.SignedOutRedirectUri = openIdConnectOptions.SignedOutRedirectUri;
                options.MapInboundClaims = openIdConnectOptions.MapInboundClaims;
                options.GetClaimsFromUserInfoEndpoint = openIdConnectOptions.GetClaimsFromUserInfoEndpoint;
                options.SaveTokens = openIdConnectOptions.SaveTokens;
                
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true
                };
                
                options.NonceCookie.SameSite = SameSiteMode.Unspecified;
                options.CorrelationCookie.SameSite = SameSiteMode.Unspecified;
            });
        
        return builder;
    }
}