using CoBySi.Pomodoro.Repository.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace CoBySi.Pomodoro.Web.Components.Account
{
    internal sealed class IdentityUserAccessor(UserManager<PomodoroUser> userManager, IdentityRedirectManager redirectManager)
    {
        public async Task<PomodoroUser> GetRequiredUserAsync(HttpContext context)
        {
            var user = await userManager.GetUserAsync(context.User);

            if (user is null)
            {
                redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
            }

            return user;
        }
    }
}
