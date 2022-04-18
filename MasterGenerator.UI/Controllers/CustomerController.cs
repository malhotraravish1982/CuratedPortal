using AutoMapper;
using MasterGenerator.Data.Entity;
using MasterGenerator.Data.Repository;
using MasterGenerator.Model.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;
using System.Collections;

namespace MasterGenerator.UI.Controllers
{
   
    public class CustomerController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public CustomerController(UserManager<AppUser> userManager,
           SignInManager<AppUser> signInManager, ILogger<HomeController> logger,
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
        [Authorize(Roles = "Admin,CS User")]
        public IActionResult GetAllCustomers()
        {
            ViewBag.DataSource = _unitOfWork.ICustomerRepository.GetCustomer();
            return View(); 
        }
        public IActionResult UrlDatasource([FromBody] Extensions.DataManagerRequestExtension dm)
        {
            string? scfFileId = dm.Table;
            IEnumerable<CustomerModel> projectRecords = null;
            projectRecords = _unitOfWork.ICustomerRepository.GetCustomer();
            if (!string.IsNullOrEmpty(scfFileId))
            {
                projectRecords = projectRecords.Where(x => x.CustomerId == Convert.ToDecimal(scfFileId));
            }

            if (!string.IsNullOrEmpty(dm.ProjectName))
            {
                System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(dm.ProjectName.ToLower());
                projectRecords = projectRecords.Where(x => x.CustomerName != null && regEx.IsMatch(x.CustomerName.ToLower()));
            }       

            IEnumerable DataSource = projectRecords;
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
    }
}
