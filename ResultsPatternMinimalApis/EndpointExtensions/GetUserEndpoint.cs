using System.Net;
using Microsoft.AspNetCore.Mvc;
using ResultsPatternMinimalApis.Services;

namespace ResultsPatternMinimalApis.EndpointExtensions;

public static class GetUserEndpoint
{
    public static IEndpointRouteBuilder GetUserByIdAsync(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/users/{id}", async ([FromRoute] int id, [FromServices] IUserService userService) =>
        {
            var result = await userService.GetUserByIdAsync(id);

        });

        return endpoints;
    }
}