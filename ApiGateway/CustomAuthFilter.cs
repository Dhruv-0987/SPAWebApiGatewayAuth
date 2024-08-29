using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiGateway;

public class CustomAuthFilter: IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.User.Identity is { IsAuthenticated: false })
        {
            context.Result = new UnauthorizedResult();
        }
    }
}