using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IdentityServerWithAspIdAndEF.Models;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using IdentityServerWithAspIdAndEF.Services;
using Microsoft.AspNetCore.Authorization;
using Repository.Interfaces;
using AccessData;
using AccessData.Models;
using PYS.IdentityServer.Security.Administration.Utilities;
using PYS.IdentityServer.Security.Administration.Models;
//using Repository.Interfaces;
namespace PYS.IdentityServer.Security.Administration.Authorize.Application
{
    public class ApplicationController : Controller
    {

        #region private variables
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;
        IApplicationRepository _applicationRepository = null;
        IAppClaimRepository _appClaimRepository = null;
        #endregion
        public ApplicationController(
        //UserManager<ApplicationUser> userManager,
        //SignInManager<ApplicationUser> signInManager,
        IApplicationRepository applicationRepository,
        IAppClaimRepository appClaimRepository,
        IIdentityServerInteractionService interaction,
        IClientStore clientStore,
        IAuthenticationSchemeProvider schemeProvider,
        IEventService events,
        IEmailSender emailSender,
        ILogger<ApplicationController> logger)
        {
            //_userManager = userManager;
            //_signInManager = signInManager;
            _applicationRepository = applicationRepository;
            _appClaimRepository = appClaimRepository;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            _logger = logger;
            _emailSender = emailSender;
        }
        public async Task<IActionResult> Index()
        {

            var list = await _applicationRepository.FindAllAsync();
            //List<ApplicationUser> users = _userManager.Users.ToList();
            //UsersViewModel uvm = new UsersViewModel() { Users = list.ToList() };
            return View(list);
        }
        [HttpGet]
        public ActionResult Register()
        {
            HttpContext.Session.SetObjectAsJson("app", null);
            return View(new ApplicationViewModel
            {
                AppClaims = new List<AppClaims>() 
            });
            
        }
        public IActionResult Create(Aplication appModel)
        {
            try
            {
                if (!string.IsNullOrEmpty(appModel.Name))
                {

                    _applicationRepository.Create(appModel);
                    _applicationRepository.SaveAsync().GetAwaiter().GetResult();
                    if (appModel.Id != 0)
                    {
                        var appList = HttpContext.Session.GetObjectFromJson<List<AppClaims>>("app");
                        if (appList != null)
                        {
                            appList.ForEach(m =>
                            {
                                m.ApplicationId = appModel.Id;
                                _appClaimRepository.Create(m);
                                _appClaimRepository.SaveAsync().GetAwaiter().GetResult();
                            });
                        }
                    }
                }
                else
                {
                    return Json(ServiceResponse.GetErrorResponse("Los datos ingresados no son válidos"));

                }
            }
            catch (Exception ex)
            {
                return Json(ServiceResponse.GetErrorResponse(ex.ToString(), null));
            }


            return Json(ServiceResponse.GetSuccessfulResponse("Los datos se ingresaron correctamente"));

        }
        public ActionResult AddAppClaim()
        { 
            return  PartialView();
        }

        [HttpGet]
        public ActionResult AppClaimList()
        {
            var appList = HttpContext.Session.GetObjectFromJson<List<AppClaims>>("app");
            if (appList == null)
                appList = new List<AppClaims>();
            return PartialView(appList);
        }

        [HttpPost]
        public ActionResult AddAppClaim(AppClaims app)
        {
            var apoList = HttpContext.Session.GetObjectFromJson<List<AppClaims>>("app");
            if (apoList != null)
            {
                apoList.Add(app);
                HttpContext.Session.SetObjectAsJson("app", apoList);
            }
            else
            {
                List<AppClaims> aplicationList = new List<AppClaims>();
                aplicationList.Add(app);
                HttpContext.Session.SetObjectAsJson("app", aplicationList);
            }

            //return Json(ServiceResponse.GetSuccessfulResponse());
            return Json(new { responseCode = 0});
        }
    }
}