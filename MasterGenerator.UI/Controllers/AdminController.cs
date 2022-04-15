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
                await _unitOfWork.CustomerMapRepository.AddCustomerMap(result);

            }
            return View();
        }
        public IActionResult CustomerMapDataSource([FromBody] Extensions.DataManagerUserExtention dm)
        {
            string? costomerId = dm.Table;
            IEnumerable<CustomerModel> customerMap = null;
            customerMap = _unitOfWork.CustomerMapRepository.GetCutomerMaped();
            if (!string.IsNullOrEmpty(costomerId))
            {
                customerMap = customerMap.Where(x => x.CustomerId == Convert.ToInt32(costomerId));
            }

            if (!string.IsNullOrEmpty(dm.FirstName))
            {
                System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(dm.FirstName.ToLower());
                customerMap = customerMap.Where(x => x.UserName != null && regEx.IsMatch(x.UserName.ToLower()));
            }

            IEnumerable DataSource = customerMap;
            DataOperations operation = new DataOperations();
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting   
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            else
            {
                Sort sort = new Sort();
                List<Sort> sorts = new List<Sort>();
                sort.Name = "Id";
                sort.Direction = "descending";
                sorts.Add(sort);
                DataSource = operation.PerformSorting(DataSource, sorts);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering   
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Cast<CustomerModel>().Count();
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public async Task<IActionResult> DeleteCustomerMapping([FromBody] ICrudModel<CustomerModel> value)
        {
            if (value.action == "remove")
            {
                if (value.key != null)
                {
                    int key = Convert.ToInt32(value.key);
                    var customerMap = await _unitOfWork.CustomerMapRepository.GetCustomerMappingById(key);
                    if (customerMap != null)
                    {
                        _unitOfWork.CustomerMapRepository.DeleteCustomerMapping(customerMap);
                        await _unitOfWork.Complete();
                        return RedirectToAction("CustomerMapping", "Admin");
                    }
                }
            }
            return Json(value.value);
        }
        [Authorize(Roles ="Admin")]
        public IActionResult ReadFile()
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
