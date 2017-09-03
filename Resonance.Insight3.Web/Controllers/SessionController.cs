using System;
using System.Web.Mvc;
using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.FormAuthentication;
using Resonance.Insight3.Web.Helpers;
using Resonance.Insight3.Web.ViewModels;

namespace Resonance.Insight3.Web.Controllers
{
    public class SessionController : Controller
    {
        /// <summary>
        ///     Creates new instance of the service
        /// </summary>
        private ICertonaService foresightService;

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

        public ICertonaService ForesightService
        {
            set { foresightService = value; } // Dependancy Injection - Called from test framework only 
            get
            {
                if (foresightService == null)
                    foresightService = new CertonaServiceClient();
                return foresightService;
            }
        }

        /// <summary>
        ///     Content partial view
        /// </summary>
        /// <returns>Return partial view</returns>
        [HttpGet]
        public PartialViewResult Warning()
        {
            return PartialView("Warning");
        }

        /// <summary>
        ///     Content partial view
        /// </summary>
        /// <returns>Return partial view</returns>
        [HttpGet]
        public PartialViewResult Timeout()
        {
            var model = new LoginViewModel();
            UserDTO user = FormsAuthenticationWrapper.User;
            if (user != null)
            {
                model.UserName = user.UserID;
                FormsAuth.SignOut();
            }
            return PartialView("Timeout", model);
        }

        /// <summary>
        ///     Timeout
        /// </summary>
        /// <param name="account">account</param>
        /// <param name="userName">userName</param>
        /// <param name="password">password</param>
        /// <returns>For valid credentials returns 'success', invalid returns error message</returns>
        [HttpPost]
        public ActionResult Timeout(string account, string userName, string password)
        {
            var model = new LoginViewModel {UserName = userName, Password = password};
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new UserDTO {UserID = model.UserName, Password = model.Password};
                    var request = new AuthenticateUserRequest {User = user};
                    AuthenticateUserResponse response = ForesightService.AuthenticateUser(request);
                    if (!response.Success)
                    {
                        ModelState.AddModelError("", Utilities.ParseServiceErrors(response.Errors));
                        return Json(Utilities.ParseServiceErrors(response.Errors));
                    }
                    if (response.User == null || response.User.UserID == null)
                    {
                        ModelState.AddModelError("", "Invalid user account.");
                        return Json("Invalid user account.");
                    }
                    FormsAuth.SetAuthCookie(response.User, false);
                    return Json("Success");
                }
                catch (Exception ex)
                {
                    //LoggingHelper.Logger.LogException(ex, typeof(AuthenticationController), "In Login method");
                    LoggingHelper.Logger.WriteException(ex);
                    return Json(ex.Message);
                }
            }
            else
            {
                return Json(Utilities.Errors(ModelState));
            }
        }

        /// <summary>
        ///     This is used from JavaScript to re-establish the user's session
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")] // Never Cache
        public ActionResult Extend()
        {
            return new EmptyResult();
        }
    }
}