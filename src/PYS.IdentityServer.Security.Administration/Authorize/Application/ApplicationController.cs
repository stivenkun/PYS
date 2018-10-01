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
        #endregion
        public ApplicationController(
        //UserManager<ApplicationUser> userManager,
        //SignInManager<ApplicationUser> signInManager,
        IApplicationRepository applicationRepository,
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
            return View();
        }
        public IActionResult Create(Aplication appModel)
        {
            return View();

        }
    }
}