using AutoMapper;
using MasterGenerator.Data.Entity;
using MasterGenerator.Data.Repository;
using MasterGenerator.Model.Model;
using MasterGenerator.UI.Helper;
using Microsoft.AspNetCore.Mvc;

namespace MasterGenerator.UI.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _hostingEnv;
        
        public AdminController(ILogger<HomeController> logger,
           IUnitOfWork unitOfWork,
           IMapper mapper,
           IWebHostEnvironment env)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hostingEnv = env;
        }
        public IActionResult CustomerMapping()
        {
            ViewBag.users = _unitOfWork.Userrepository.GetUsers();
            ViewBag.customers = _unitOfWork.CustomerRepository.GetAllCustomers();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CustomerMappingAsync(CustomerModel customerModel)
        {
            ViewBag.users = _unitOfWork.Userrepository.GetUsers();
            ViewBag.customers = _unitOfWork.CustomerRepository.GetAllCustomers();

            var result = _mapper
                    .Map<CustomerMap>(customerModel);
            if (result != null)
            { 
                await _unitOfWork.ICustomerMapRepository.AddCustomerMap(result);
                
            }
            return View();
        }
    }
}
