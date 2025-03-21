using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using WebsiteBanHang.Models;

public class EnsureEmailConfirmedMiddleware
{
    private readonly RequestDelegate _next;

    public EnsureEmailConfirmedMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        if (context.User.Identity.IsAuthenticated)
        {
            var user = await userManager.GetUserAsync(context.User);
            if (user != null && !await userManager.IsEmailConfirmedAsync(user))
            {
                await signInManager.SignOutAsync();
                context.Response.Redirect("/Account/EmailVerificationPending?email=" + user.Email);
                return;
            }
        }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        await _next(context);
    }
}
