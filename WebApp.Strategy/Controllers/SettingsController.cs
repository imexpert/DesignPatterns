using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApp.Strategy.Models;

namespace WebApp.Strategy.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public SettingsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            Settings settings = new();

            if (User.Claims.FirstOrDefault(s => s.Type == Settings.ClaimDatabaseType) != null)
            {
                settings.DatabaseType = (DatabaseTypes)int.Parse(User.Claims.FirstOrDefault(s => s.Type == Settings.ClaimDatabaseType)?.Value ?? string.Empty);
            }
            else
            {
                settings.DatabaseType = settings.GetDefaultDatabaseType;
            }

            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDatabase(int databaseType)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var newClaim = new Claim(Settings.ClaimDatabaseType, databaseType.ToString());
            var claims = await _userManager.GetClaimsAsync(user);
            var hasDatabaseTypeClaim = claims.FirstOrDefault(s => s.Type == Settings.ClaimDatabaseType);
            if (hasDatabaseTypeClaim != null)
            {
                await _userManager.ReplaceClaimAsync(user, hasDatabaseTypeClaim, newClaim);
            }
            else
            {
                await _userManager.AddClaimAsync(user, newClaim);
            }

            await _signInManager.SignOutAsync();
            var result = await HttpContext.AuthenticateAsync();

            await _signInManager.SignInAsync(user, result.Properties);
            return RedirectToAction(nameof(Index));
        }
    }
}
