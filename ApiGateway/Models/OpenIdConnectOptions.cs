using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Models;

public class OpenIdConnectOptions
{
    [Required]
    public string? Authority { get; set; }

    [Required]
    public string? ClientId { get; set; }

    public string ResponseType { get; set; } = default!;

    public string ResponseMode { get; set; } = default!;

    [MinLength(3)]
    public List<string> Scope { get; set; } = [];

    [Required]
    public string? CallbackPath { get; set; }

    [Required]
    public string SignedOutRedirectUri { get; set; } = default!;

    [Required]
    public bool MapInboundClaims { get; set; }

    [Required]
    public bool GetClaimsFromUserInfoEndpoint { get; set; }

    [Required]
    public bool SaveTokens { get; set; }
}