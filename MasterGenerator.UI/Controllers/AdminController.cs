using AutoMapper;
using MasterGenerator.Data.Entity;
using MasterGenerator.Data.Repository;
using MasterGenerator.Model.Model;
using MasterGenerator.UI.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Base;
using System.Collections;

namespace MasterGenerator.UI.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private IWebHostEnvironment _hostingEnv;
        private readonly RoleManager<AppRole> _roleManager;

        public AdminController(RoleManager<AppRole> roleManager,UserManager<AppUser> userManager, ILogger<HomeController> logger,
           IUnitOfWork unitOfWork,
           IMapper mapper,
           UserManager<AppUser> userManager,
           IWebHostEnvironment env)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _hostingEnv = env;
            _roleManager = roleManager;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUser()
        {
            ViewBag.Roles = await _roleManager.Roles.Select(x => x.Name).ToListAsync();
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUser(UserModel userModel)
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
                var roleResult = await _userManager.AddToRoleAsync(user, userModel.UserType);

                if (!roleResult.Succeeded)
                {
                    string errors = string.Empty;
                    foreach (var error in result.Errors)
                    {
                        errors += error.Description + Environment.NewLine;
                    }
                    ModelState.AddModelError("", errors);
                    return View(userModel);
                }
            }
            return RedirectToAction("AddUser", "Admin");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult CustomerMapping()
        {
            ViewBag.user = _unitOfWork.Userrepository.GetUsersByRole(AdminEnum.Customer_User.ToString().Replace("_", " "));
            ViewBag.customers = _unitOfWork.CustomerRepository.GetAllCustomers();
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CustomerMappingAsync(CustomerModel customerModel)
        {
            ViewBag.user = _unitOfWork.Userrepository.GetUsersByRole(AdminEnum.Customer_User.ToString().Replace("_", " "));
            ViewBag.customers = _unitOfWork.CustomerRepository.GetAllCustomers();

            var result = _mapper
                    .Map<CustomerMap>(customerModel);
            if (result != null)
            { 
                await _unitOfWork.ICustomerMapRepository.AddCustomerMap(result);
                
            }
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
