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

            var res = _unitOfWork.Userrepository.FindUserRoleById(user.Id);
            if (res.Result != null)
            {
                int i = res.Result.RoleId;
                if (i == 1)
                    return RedirectToAction("GetAllCustomers", "Customer");
                else if (i == 2)
                    return RedirectToAction("GetAllCustomers", "Customer");
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
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
