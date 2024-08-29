using Microsoft.Extensions.Options;
using WebApplication1;

namespace ApiGateway.HostingExtensions;

public static class ProxyExtensions
{
    public static WebApplicationBuilder ConfigureYarpProviders(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<ProxyConfigOptions>()
            .BindConfiguration(nameof(ProxyConfigOptions))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var proxyConfigOptions = builder.Services.BuildServiceProvider()
            .GetRequiredService<IOptions<ProxyConfigOptions>>().Value;

        builder.Services.AddReverseProxy()
            .LoadFromMemory(proxyConfigOptions.Routes, proxyConfigOptions.Clusters)
            .AddTransforms<AccessTokenTransformProvider>();

        return builder;
    }

    public static WebApplication ConfigureYarpMiddleware(this WebApplication app)
    {
        app.MapReverseProxy();
        return app;
    }
}