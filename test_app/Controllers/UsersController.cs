using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test_app.Models;
using test_app.Services;

namespace test_app.Controllers
{
    public class UsersController : Controller
    {
        private readonly DBService _db;
        private readonly PasswordHasher<string> _passwordHasher;

        public UsersController(DBService db)
        {
            _db = db;
            _passwordHasher = new PasswordHasher<string>();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            else
            {
                var dbUser = await _db.GetUserByEmailAsync(user.Email);
                if(dbUser == null)
                {
                    ModelState.AddModelError("", "Wrong email");
                    return View(user);
                }

                var result = _passwordHasher.VerifyHashedPassword(null, dbUser.Password, user.Password);

                if (result != PasswordVerificationResult.Success)
                {
                    ModelState.AddModelError("", "Wrong password");
                    return View(user);
                }
                return RedirectToAction("Index", "Tasks");
            }
            
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);              
            }
            else
            {
                if(_db.CheckUser(user.Email))
                {
                    ModelState.AddModelError("", "User already available");
                    return View(user);
                }

                string hash = _passwordHasher.HashPassword(null, user.Password);
                user.Password = hash;
                await _db.AddUserAsync(user);
                return RedirectToAction(nameof(Login));
            }          
        }
    }
}
