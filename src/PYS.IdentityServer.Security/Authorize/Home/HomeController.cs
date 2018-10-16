// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;
using System.Threading.Tasks;

namespace IdentityServer4.Authorize.UI
{
    //[SecurityHeaders]
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IApplicationRepository _applicationRepository;

        public HomeController(IIdentityServerInteractionService interaction
            ,IApplicationRepository applicationRepository)
        {
            _interaction = interaction;
            _applicationRepository = applicationRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ApplicationMenu()
        {
            var aplicaciones = _applicationRepository.FindAllAsync().GetAwaiter().GetResult();
            return View(aplicaciones);
        }
        /// <summary>
        /// Shows the error page
        /// </summary>
        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;
            }

            return View("Error", vm);
        }
    }
}