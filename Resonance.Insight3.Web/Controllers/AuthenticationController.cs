using System;
using System.Globalization;
using System.Web.Mvc;
using Resonance.Insight3.Web.FormAuthentication;
using Resonance.Insight3.Web.Helpers;
using Resonance.Insight3.Web.Models;
using Resonance.Insight3.Web.ViewModels;

namespace Resonance.Insight3.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        /// <summary>
        ///     Creates new instance of FormsAuthenticationWrapper
        /// </summary>
        private IFormsAuthentication formsAuth;
        public IFormsAuthentication FormsAuth
        {
            set { formsAuth = value; } // Dependancy Injection - Called from test framework only 
            get
            {
                if (formsAuth == null)
                    formsAuth = new FormsAuthenticationWrapper();
                return formsAuth;
            }
        }

        /// <summary>
        ///     Login view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            var viewModel = new ViewModels.LoginViewModel();

            if (!string.IsNullOrWhiteSpace(Request.QueryString["SessionTimeout"]))
            {
                ModelState.AddModelError("error", Resource.SessionExpiredText);
                ViewBag.Title = "Login";
                return View("Login", viewModel);
            }

            if (!string.IsNullOrWhiteSpace(Request.QueryString["Logout"]))
            {
                viewModel.SuccessMessage = Resource.LogoutText;
            }

            ViewBag.Title = "Login";
            return View("Login", viewModel);
        }

        /// <summary>
        ///     Called when user clicks on login button.
        /// </summary>
        /// <param name="model">LoginViewModel</param>
        /// <returns>For valid credentials redirects to Home else returns login view with appropriate message</returns>
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var authRet = AuthenticationModel.AuthenticateUser(model);

                    if (String.IsNullOrWhiteSpace(authRet))
                    {
                        return Redirect("~/Home");
                    }
                    
                    ModelState.AddModelError(string.Empty, authRet);
                    return View("Login", model);
                }
                catch (Exception ex)
                {
                    //LoggingHelper.Logger.LogException(ex, typeof(AuthenticationController), "In Login method");
                    LoggingHelper.Logger.WriteException(ex);
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View("Login", model);
        }

        /// <summary>
        ///     Logoff - Clears sessions and redirects to login screen
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult Logoff()
        {
            FormsAuth.SignOut();
            return RedirectToAction("Login", "Authentication", new { Logout=true });
        }
    }
}