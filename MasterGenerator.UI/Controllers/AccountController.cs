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
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager,ILogger<HomeController> logger, 
            IUnitOfWork unitOfWork, IMapper mapper,
            RoleManager<AppRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Import", "Home");
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
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
           _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Register()
        {
            ViewBag.Roles = await _roleManager.Roles.ToListAsync();
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(UserModel userModel)
        {
            ViewBag.Roles = await _roleManager.Roles.Select(x => x.Name).ToListAsync();
            if (ModelState.IsValid)
            {
                if (await UserExists(userModel.Email))
                {
                    ModelState.AddModelError("", "Email already taken.");
                    return View(userModel);
                }

                if (await UserNameExists(userModel.Username))
                {
                    ModelState.AddModelError("", "User Name already taken.");
                    return View(userModel);
                }

                var user = new AppUser();
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.Address = userModel.Address;
                user.Email = userModel.Email.ToLower();
                user.UserName = userModel.Username.ToLower();
                user.PhoneNumber = userModel.PhoneNumber;

                var result = await _userManager.CreateAsync(user, userModel.Password);

                if (!result.Succeeded)
                {
                    string errors = string.Empty;
                    foreach (var error in result.Errors)
                    {
                        errors += error.Description + Environment.NewLine;
                    }
                    ModelState.AddModelError("", errors);
                    return View(userModel);
                } 
                var roleResult = await _userManager.AddToRoleAsync(user, "Admin");
               
                if (!roleResult.Succeeded)
                {
                    string errors = string.Empty;
                    foreach (var error in result.Errors)
                    {
                        errors += error.Description + Environment.NewLine;
                    }
                    ModelState.AddModelError("", errors);
                    return RedirectToAction("Login");
                }
            }
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
        #region "private Methods"
        private async Task<bool> UserExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }
        private async Task<bool> UserNameExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
        #endregion
    }
}
