using Microsoft.AspNetCore.Identity;

namespace TestAuth.Authorization;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    //private readonly UserManager<IdentityUser> _userManager;


    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
        //_userManager = userManager;
    }

    public async Task Invoke(HttpContext context, IJwtUtils jwtUtils, UserManager<IdentityUser> userManager)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtUtils.ValidateJwtToken(token);

        if (userId != null)
        {
            // attach user to context on successful jwt validation
            context.Items["User"] = await userManager.FindByIdAsync(userId);
        }

        await _next(context);
    }
}