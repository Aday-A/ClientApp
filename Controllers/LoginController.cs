using System;
using ClientApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Internal;
using Microsoft.AspNetCore.Mvc;
using System.DirectoryServices.AccountManagement;


namespace ClientApp.Controllers
{
    
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ActionResult Login()
        {
            ViewBag.Error = string.Empty;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            try
            {
                string domainIP = _configuration.GetSection("Domain").GetValue<string>("ADIP")!;
                using PrincipalContext pc = new PrincipalContext(ContextType.Domain, domainIP);

                bool isValid = pc.ValidateCredentials("username", "password");
                if(isValid)
                {
                    return RedirectToAction("/Users");
                }
                else
                {
                    ViewBag.Error = "Invalid login details";
                    return View();
                }

            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;
                return View();
            }
        }
    }        
}
