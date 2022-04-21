using AutoMapper;
using Google.Apis.Sheets.v4;
using MasterGenerator.Data.Context;
using MasterGenerator.Data.Entity;
using MasterGenerator.Data.Repository;
using MasterGenerator.Model.Model;
using MasterGenerator.UI.Helper;
using MasterGenerator.UI.Mapper;
using MasterGenerator.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using Syncfusion.EJ2.Base;
using System.Collections;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace MasterGenerator.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _hostingEnv;
        private readonly UserManager<AppUser> _userManager;

        #region google properties
        private const string SpreadsheetId = "1yJRNyvwJJr-QJaflsTLeMZdw6cUMEMNLsUf_501FTJk";
        private const string GoogleCredentialsFileName = "google-credentials.json";
        private const string ReadRangeForPortalData = "Portal Data!A:R";
        private const string ReadRangeForDealDetails = "Deal Details!A:J";
        SpreadsheetsResource.ValuesResource _googleSheetValues;
        #endregion

        public HomeController(ILogger<HomeController> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            UserManager<AppUser> userManager,
            IWebHostEnvironment env,
            GoogleSheetsHelper googleSheetsHelper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hostingEnv = env;
            _userManager = userManager;
            _googleSheetValues = googleSheetsHelper.Service.Spreadsheets.Values;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Customer User")]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                var customerNameList = await _unitOfWork.ICustomerRepository.GetCustomerNamesByUserId(int.Parse(userId));
                if (customerNameList != null)
                {
                    ViewBag.DataSource =await _unitOfWork.IDealDetailsRepository.GetDealDetailsByCustomerNames(customerNameList);
                }
                ViewBag.statusList = await _unitOfWork.IProjectRepository.GetProjectStatus();
                ViewBag.Project = _unitOfWork.IProjectRepository.GetProjectsByCustomerNames(customerNameList);
            }
            return View(); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dm"></param>
        /// <returns></returns>
        public async Task<IActionResult> UrlDatasource([FromBody] Extensions.DataManagerRequestExtension dm)
        {
          
            var userId = _userManager.GetUserId(User);

            if (userId != null)
            {
                string? scfFileId = dm.Table;
                IEnumerable<ProjectModel> projectRecords = null;
                IEnumerable<PermissionModel> projectRecordsGrantPermission = null;
                var  customerNameList = await _unitOfWork.ICustomerRepository.GetCustomerNamesByUserId(int.Parse(userId));
                //var  customerFieldListByUserId = await _unitOfWork.IProjectRepository.GetCustomerFeildByUserId(int.Parse(userId));
                if (customerNameList != null)
                {
                    projectRecords = _unitOfWork.IProjectRepository.GetProjectsByCustomerNames(customerNameList);               
                   // projectRecordsGrantPermission =  _unitOfWork.IProjectRepository.GetProjectsByVisibleFeildPermission(customerFieldListByUserId);
                }
                if (!string.IsNullOrEmpty(scfFileId))
                {
                    projectRecords = projectRecords.Where(x => x.ProjectId == Convert.ToDecimal(scfFileId));
                }
                if (!string.IsNullOrEmpty(dm.ProjectName))
                {
                    System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(dm.ProjectName.ToLower());
                    projectRecords = projectRecords.Where(x => x.ProjectName != null && regEx.IsMatch(x.ProjectName.ToLower()));
                }
                if (!string.IsNullOrEmpty(dm.PODate))
                {
                    System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(dm.PODate.ToLower());
                    projectRecords = projectRecords.Where(x => x.PODate != null && regEx.IsMatch(x.PODate.ToLower()));
                }
                IEnumerable DataSource = projectRecords;
                DataOperations operation = new DataOperations();
                if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting   
                {
                    DataSource = operation.PerformSorting(DataSource, dm.Sorted);
                }
                else
                {
                    Sort sort = new Sort();
                    List<Sort> sorts = new List<Sort>();
                    sort.Name = "ProjectId";
                    sort.Direction = "descending";
                    sorts.Add(sort);
                    DataSource = operation.PerformSorting(DataSource, sorts);


                }
                if (dm.Where != null && dm.Where.Count > 0) //Filtering   
                {
                    DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
                }
                int count = DataSource.Cast<ProjectModel>().Count();
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
            return View(Error);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //[Authorize(Roles ="Admin")]
        public async Task<IActionResult> ReadFile()
        {
            //read data from Portal data sheet
            var portalDataResult = ReadDataFromGoogleSpreadSheet(ReadRangeForPortalData);
            if (portalDataResult != null)
            {
                ProjectMapper projectMapper = new ProjectMapper(_unitOfWork);
                var projects =await projectMapper.MapFromRangeData(portalDataResult);
                if (projects.Count > 0)
                {
                    var result=await _unitOfWork.IProjectRepository.AddProjectRange(projects);
                    if (result == true)
                    {
                        //get all customers
                        var customers = projects.Where(x => !string.IsNullOrEmpty(x.CustomerName)).Select(x => x.CustomerName).Distinct().ToList();
                        if (customers.Count > 0)
                        {
                            await _unitOfWork.ICustomerRepository.AddCustomerRange(customers);
                        }
                    }

                    //read data from deal details sheet
                    var dealDetailsResult = ReadDataFromGoogleSpreadSheet(ReadRangeForDealDetails);
                    if (dealDetailsResult != null)
                    {
                        var dealDetails = DealDetailsMapper.MapFromRangeData(dealDetailsResult, projects);
                        if (dealDetails.Count > 0)
                        {
                            await _unitOfWork.IDealDetailsRepository.AddDealDetailsRange(dealDetails);
                        }
                    }
                }
            }
           
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Customer User")]
        public IActionResult MapedCustomer()
        {

            return View();
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

        #region Read Data from google spreadsheet
        private IList<IList<object>> ReadDataFromGoogleSpreadSheet(string readRange)
        {
            var request = _googleSheetValues.Get(SpreadsheetId, readRange);
            var response = request.Execute();
            if (response != null)
            {
                var values = response.Values;
                if (values.Count > 0)
                {
                    return values.Skip(1).ToList();
                }
            }
            return null;
        }
        #endregion  
    }
}