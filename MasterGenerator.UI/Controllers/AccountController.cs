using AutoMapper;
using MasterGenerator.Data.Entity;
using MasterGenerator.Data.Helper;
using MasterGenerator.Data.Repository;
using MasterGenerator.Model.Model;
using MasterGenerator.UI.Models;
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
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,ILogger<HomeController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

            //Signin successfull//
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Import", "Home");              
        }

        public IActionResult Logout()
        {
           _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public async Task<IActionResult> Register(string email,string password,string firstName,string lastName)
        {
            
            if (await UserExists(email)) return BadRequest("Email is taken");

            var user = new AppUser();

            user.Email = email.ToLower();
            user.UserName = email.ToLower();

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Admin");

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        private async Task<bool> UserExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }
    }
}
