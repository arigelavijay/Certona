using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Resonance.Insight3.Web.HtmlHelpers;

//using Resonance.Utilities.Logging;

namespace Resonance.Insight3.Web.Controllers
{
    [Localization]
    public class ControllerBase : Controller
    {
        private HttpRequestBase request;
        private HttpResponseBase response;

        /// <summary>
        ///     Static Logger used for all controllers LogMessage is equivalent to Info logging
        ///     LogException is equivalent to Error Loggign
        /// </summary>
        //public static ILogger Logger = new Logger();
        protected override void Initialize(RequestContext requestContext)
        {
            request = requestContext.HttpContext.Request;
            response = requestContext.HttpContext.Response;

            base.Initialize(requestContext);
        }

        /// Summary
        /// Set the culture of the thread based on the language selction menu
        /// Summary
        public ActionResult ChangeLanguage(string language)
        {
            if (request.Cookies.AllKeys.Contains("Certona.Insight3.Language"))
            {
                HttpCookie lang = request.Cookies.Get("Certona.Insight3.Language");
                lang.Value = language;
                lang.Expires = DateTime.Now.AddDays(30);
                response.Cookies.Set(lang);
            }
            else
            {
                var lang = new HttpCookie("Certona.Insight3.Language", language);
                lang.Expires = DateTime.Now.AddDays(30);
                response.Cookies.Add(lang);
            }

            return Json(new {IsSuccess = true}, JsonRequestBehavior.AllowGet);
        }
    }
}