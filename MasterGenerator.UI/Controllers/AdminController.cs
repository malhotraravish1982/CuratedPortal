using AutoMapper;
using MasterGenerator.Data.Entity;
using MasterGenerator.Data.Repository;
using MasterGenerator.Model.Model;
using MasterGenerator.UI.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;
using System.Collections;

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
        //private readonly CustomerManager<CustomerModel> _customerManager;
        public AdminController(ILogger<HomeController> logger,
           IUnitOfWork unitOfWork,
           IMapper mapper,
           UserManager<AppUser> userManager,
           IWebHostEnvironment env)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hostingEnv = env;
            _userManager = userManager;
        }
        public IActionResult CustomerMapping()
        {
            ViewBag.users = _unitOfWork.Userrepository.GetUsers();
            ViewBag.customers = _unitOfWork.IDealDetailsRepository.GetAllCustomers();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CustomerMappingAsync(CustomerModel customerModel)
        {
            ViewBag.users = _unitOfWork.Userrepository.GetUsers();
            ViewBag.customers = _unitOfWork.IDealDetailsRepository.GetAllCustomers(); 
                        
               var result = _mapper
                    .Map<CustomerMap>(customerModel);
            if (result != null)
            { 
                await _unitOfWork.ICustomerMapRepository.AddCustomerMap(result);
                
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
            UserRecords = _unitOfWork.Userrepository.GetUsers();
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
        public async Task<IActionResult> CrudUpdate([FromBody] Model.Model.CRUDModel<UserModel> value, string action)
        {
            
            
            if (value.action == "update")
            {
                if (value.value != null)
                {
                    var User = await _unitOfWork.Userrepository.GetUserById(Convert.ToInt32( value.key));
                    _mapper.Map(value.value, User, typeof(UserModel), typeof(AppUser));
                    try
                    {
                        _unitOfWork.Userrepository.Update(User);
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
                    var csUser = await _unitOfWork.Userrepository.GetUserById(Convert.ToInt32(value.key));
                    _unitOfWork.Userrepository.Delete(csUser);
                    await _unitOfWork.Complete();
                    return Json(value);
                }
            }
            return Json(value.value);
        }


    }
}
