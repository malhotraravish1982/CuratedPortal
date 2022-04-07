using AutoMapper;
using MasterGenerator.Data.Repository;
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
            return View();
        }
    }
}
