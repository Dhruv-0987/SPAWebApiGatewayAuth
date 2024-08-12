using System.Net;
using Microsoft.AspNetCore.Mvc;
using ResultsPatternMinimalApis.Models;
using ResultsPatternMinimalApis.Services;

namespace ResultsPatternMinimalApis.EndpointExtensions;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/users/{id}", async ([FromRoute] int id, [FromServices] IUserService userService) =>
            {
                var result = await userService.GetUserByIdAsync(id);
                return result.ToMinimalApiResult();
            })
            .Produces(StatusCodes.Status200OK, typeof(User))
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);

        endpoints.MapGet("/api/users", async ([FromServices] IUserService userService) =>
            {
                var result = await userService.GetUsersAsync();
                return result.ToMinimalApiResult();
            })
            .Produces(StatusCodes.Status200OK, typeof(List<User>))
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);

        endpoints.MapPost("/api/users", async ([FromBody] User user, [FromServices] IUserService userService) =>
            {
                var result = await userService.CreateUserAsync(user);
                return result.ToMinimalApiResult();
            })
            .Produces(StatusCodes.Status201Created, typeof(User))
            .Produces(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status401Unauthorized);

        endpoints.MapPut("/api/users", async ([FromBody] User user, [FromServices] IUserService userService) =>
            {
                var result = await userService.UpdateUserAsync(user);
                return result.ToMinimalApiResult();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);

        return endpoints;
    }
}