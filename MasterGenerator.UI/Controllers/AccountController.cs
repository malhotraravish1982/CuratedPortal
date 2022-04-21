using AutoMapper;
using MasterGenerator.Data.Entity;
using MasterGenerator.Data.Helper;
using MasterGenerator.Data.Repository;
using MasterGenerator.Model.Model;
using MasterGenerator.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MasterGenerator.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(ILogger<HomeController> logger,UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
            {
            //    if (userRoles[0] == AdminEnum.Admin.ToString())
            //    {
            //        return RedirectToAction("GetAllCustomers", "Customer");
            //    }
            //    else if (userRoles[0] == AdminEnum.CS_User.ToString().Replace("_", " "))
            //    {
            //        return RedirectToAction("GetAllCustomers", "Customer");
            //    }
            //    else if (userRoles[0] == AdminEnum.Customer_User.ToString().Replace("_", " "))
            //    {
            //        return RedirectToAction("Index", "Home");
            //    }
            //    return RedirectToAction("Index", "Home");
                return View();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserModel userModel)
        {
            var user = await _userManager.Users
            .SingleOrDefaultAsync(x => x.Email == userModel.Email.ToLower());
            if (user == null)
            {
                ModelState.AddModelError("Email", "Wrong email, please enter correct email.");
                return View(userModel);
            }

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, userModel.Password, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("Password", "Wrong password, please enter correct password.");
                return View(userModel);
            }

            //Signin successfull
            await _signInManager.SignInAsync(user, isPersistent: false);
            var userRoles = await _userManager.GetRolesAsync(user);
            if ((userRoles != null) && userRoles.Count > 0)
            {
                if (userRoles[0] == AdminEnum.Admin.ToString())
                {
                    return RedirectToAction("GetAllCustomers", "Customer");
                }
                else if (userRoles[0] == AdminEnum.CS_User.ToString().Replace("_", " "))
                {
                    return RedirectToAction("GetAllCustomers", "Customer");
                }
                else if (userRoles[0] == AdminEnum.Customer_User.ToString().Replace("_", " "))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(userModel);
        }

        public IActionResult Logout()
        {
           _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }       
        public IActionResult Index()
        {
            return View();
        }   
    }
}
