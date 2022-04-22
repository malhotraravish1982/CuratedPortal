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
using System.Collections.Generic;

namespace MasterGenerator.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private IWebHostEnvironment _hostingEnv;
        private readonly RoleManager<AppRole> _roleManager;

        public AdminController(RoleManager<AppRole> roleManager,
            UserManager<AppUser> userManager, 
            ILogger<HomeController> logger,
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
       
        public async Task<IActionResult> AddUser()
        {
            ViewBag.Roles = await _roleManager.Roles.Where(x => x.Name!="Admin").ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(UserModel userModel)
        {
            ViewBag.Roles = await _roleManager.Roles.Where(x => x.Name != "Admin").ToListAsync();
            
            if (ModelState.IsValid)
            {
                if (await UserExists(userModel.Email))
                {
                    ModelState.AddModelError("", "Email already taken.");
                    return View(userModel);
                }
                var user = new AppUser();
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.Address = userModel.Address;
                user.Email = userModel.Email.ToLower();
                user.UserName = userModel.FirstName.ToLower();
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
                return RedirectToAction("AddUser","Admin");
            }
            return View();
        }
        public IActionResult CustomerMapping()
        {
            ViewBag.user = _unitOfWork.IUserrepository.GetUsersByRole(AdminEnum.Customer_User.ToString().Replace("_", " "));
            ViewBag.customers = _unitOfWork.ICustomerRepository.GetAllCustomers();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CustomerMappingAsync(CustomerModel customerModel)
        { 
            if (ModelState.IsValid)
            {
                 var mapCustomer = _unitOfWork.ICustomerMapRepository.GetMappingRecordById(customerModel);
                if (mapCustomer != null)
                {
                    ModelState.AddModelError("", "Customer already maped");
                    return View(customerModel);
                }
                var result = _mapper
                        .Map<CustomerMap>(customerModel);
                if (result != null)
                {
                    await _unitOfWork.ICustomerMapRepository.AddCustomerMap(result);

                }
                return RedirectToAction("CustomerMapping");
            }

            return View();
        }
        public async Task<IActionResult> GetUsers()
        {
            return View();
        }
        
        public IActionResult UrlDatasource([FromBody] Extensions.DataManagerRequestExtension dm)
        {
            string? FileId = dm.Table;
            IEnumerable<UserModel> UserRecords = null;
            UserRecords = _unitOfWork.IUserrepository.GetUsers();
            if (!string.IsNullOrEmpty(FileId))
            {
                UserRecords = UserRecords.Where(x => x.Id == Convert.ToDecimal(FileId));
            }

            if (!string.IsNullOrEmpty(dm.ProjectName))
            {
                System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(dm.ProjectName.ToLower());
                UserRecords = UserRecords.Where(x => x.Username != null && regEx.IsMatch(x.Username.ToLower()));
            }
            IEnumerable DataSource = UserRecords;
            DataOperations operation = new DataOperations();
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting   
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            else
            {
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering   
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Cast<UserModel>().Count();
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
        
        public IActionResult UrlVisibleFieldDataSource([FromBody] Extensions.DataManagerRequestExtension dm)
        {
            string? FileId = dm.Table;
            IEnumerable<PermissionModel> UserPermissionRecords = null;
            UserPermissionRecords = _unitOfWork.IUserPermissionRepository.GetUserPermissionRecord();
            if (!string.IsNullOrEmpty(FileId))
            {
                UserPermissionRecords = UserPermissionRecords.Where(x => x.Id == Convert.ToDecimal(FileId));
            }

            if (!string.IsNullOrEmpty(dm.ProjectName))
            {
                System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(dm.ProjectName.ToLower());
                UserPermissionRecords = UserPermissionRecords.Where(x => x.ProjectName != null && regEx.IsMatch(x.ProjectName.ToLower()));
            }
            IEnumerable DataSource = UserPermissionRecords;
            DataOperations operation = new DataOperations();
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting   
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            else
            {
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering   
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Cast<PermissionModel>().Count();
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
        public async Task<IActionResult> CrudUpdateVisibleField([FromBody] Model.Model.CRUDModel<PermissionModel> value, string action)
        {
            if (value.action == "update")
            {
                if (value.value != null)
                {
                    var User = await _unitOfWork.IUserPermissionRepository.GetVisibleFieldByUserId(Convert.ToInt32(value.key));
                    _mapper.Map(value.value, User, typeof(PermissionModel), typeof(FieldPermission));
                    try
                    {
                        _unitOfWork.IUserPermissionRepository.Update(User);
                        await _unitOfWork.Complete();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return Json(value.value);
        }
        public async Task<IActionResult> CrudUpdate([FromBody] Model.Model.CRUDModel<UserModel> value, string action)
        {
            if (value.action == "update")
            {
                if (value.value != null)
                {
                    var User = await _unitOfWork.IUserrepository.GetUserById(Convert.ToInt32(value.key));
                    _mapper.Map(value.value, User, typeof(UserModel), typeof(AppUser));
                    try
                    {
                        _unitOfWork.IUserrepository.Update(User);
                        await _unitOfWork.Complete();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            else if (value.action == "remove")
            {
                if (value.key != null)
                {
                    var csUser = await _unitOfWork.IUserrepository.GetUserById(Convert.ToInt32(value.key));
                    _unitOfWork.IUserrepository.Delete(csUser);
                    await _unitOfWork.Complete();
                    return Json(value);
                }
            }
            return Json(value.value);
        }
        public IActionResult CustomerMapDataSource([FromBody] Extensions.DataManagerUserExtention dm)
        {
            string? costomerId = dm.Table;
            IEnumerable<CustomerModel> customerMap = null;
            customerMap = _unitOfWork.ICustomerMapRepository.GetCutomerMaped();
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
        public async Task<IActionResult> DeleteCustomerMapping([FromBody] Model.Model.CRUDModel<CustomerModel> value)
        {
            if (value.action == "remove")
            {
                if (value.key != null)
                {
                    int key = Convert.ToInt32(value.key);
                    var customerMap = await _unitOfWork.ICustomerMapRepository.GetCustomerMappingById(key);
                    if (customerMap != null)
                    {
                        _unitOfWork.ICustomerMapRepository.DeleteCustomerMapping(customerMap);
                        await _unitOfWork.Complete();
                        return RedirectToAction("CustomerMapping", "Admin");
                    }
                }
            }
            return Json(value.value);
        }
        public IActionResult UserPermission()
        {
            ViewBag.user = _unitOfWork.IUserrepository.GetUsersByRole(AdminEnum.Customer_User.ToString().Replace("_", " "));
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UserPermissionAsync(PermissionModel permissionModel)
        {
            if (ModelState.IsValid)
            {
                var userPermission = _unitOfWork.IUserPermissionRepository.GetPermisedRecordById(permissionModel);
                if (userPermission != null)
                {
                    ModelState.AddModelError("", "User already permissed");
                    return View(permissionModel);
                }
                var result = _mapper
                        .Map<FieldPermission>(permissionModel);
                if (result != null)
                {
                    await _unitOfWork.IUserPermissionRepository.AddUserPermission(result);
                    return RedirectToAction("UserPermission");
                }
                return RedirectToAction("UserPermission");
            }
           return View(permissionModel);
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
