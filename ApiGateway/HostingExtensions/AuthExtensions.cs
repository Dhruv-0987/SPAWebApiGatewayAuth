using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApplication1;

namespace ApiGateway.HostingExtensions;

public static class AuthExtensions
{
    public static WebApplicationBuilder ConfigureAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<Models.OpenIdConnectOptions>()
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

        builder.Services.AddHttpContextAccessor();
        
        builder.Services.AddBff();
        
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("AuthenticatedUser", policy =>
            {
                policy.RequireAuthenticatedUser();
            });
        
        var openIdConnectOptions = builder.Services.BuildServiceProvider()
            .GetRequiredService<IOptions<Models.OpenIdConnectOptions>>().Value;

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = "spa_cookie";
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.SlidingExpiration = true;
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            })
            .AddOpenIdConnect(options =>
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

        builder.Services.AddSingleton<CookieOidcAndTokenRefresher>();
        builder.Services.AddOptions<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme)
            .Configure<CookieOidcAndTokenRefresher>((cookieOptions, refresher) =>
            {
                cookieOptions.Events.OnValidatePrincipal = context => 
                    refresher.ValidateOrRefreshTokenAsync(context, OpenIdConnectDefaults.AuthenticationScheme);
            });
        
        var allowedCorsOrigins = builder.Configuration.GetSection("AllowedCorsOrigins").Get<string[]>();
        
        var corsPolicy = new CorsPolicyBuilder()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithOrigins(allowedCorsOrigins!)
            .Build();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("_ApiGateway", corsPolicy);
        });
        
        return builder;
    }
    
    public static WebApplication ConfigureAuthMiddleware(this WebApplication app)
    {
        app.UseCors("_ApiGateway");
        app.UseAuthentication();
        app.UseBff();
        app.UseAuthorization();

        app.MapBffManagementEndpoints();
        return app;
    }
}