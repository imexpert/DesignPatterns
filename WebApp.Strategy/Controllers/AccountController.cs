﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Strategy.Models;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Strategy.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var hasUser = await _userManager.FindByEmailAsync(email);
            if (hasUser == null)
            {
                return View();
            }

            var siginResult = await _signInManager.PasswordSignInAsync(hasUser, password, true, false);

            if (!siginResult.Succeeded)
            {
                return View();
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
