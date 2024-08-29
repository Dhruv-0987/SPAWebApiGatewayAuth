using System.ComponentModel.DataAnnotations;
using Yarp.ReverseProxy.Configuration;

namespace WebApplication1;

public sealed class ProxyConfigOptions
{
    [MinLength(1)]
    [Required]
    public IReadOnlyList<RouteConfig> Routes { get; set; } = default!;

    [MinLength(1)]
    [Required]
    public IReadOnlyList<ClusterConfig> Clusters { get; set; } = default!;
}