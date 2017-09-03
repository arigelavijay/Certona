using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Resonance.Insight3.Web.HtmlHelpers
{
    public static class i18nHelpers
    {
        public static string LocalResources(this WebViewPage page, string key)
        {
            return page.ViewContext.HttpContext.GetLocalResourceObject(page.VirtualPath, key) as string;
        }
    }

    public class LocalizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpRequestBase request = filterContext.HttpContext.Request;
            HttpResponseBase response = filterContext.HttpContext.Response;
            string language = string.Empty;
            if (request.Cookies.AllKeys.Contains("Certona.Insight3.Language"))
            {
                HttpCookie lang = request.Cookies["Certona.Insight3.Language"];
                language = lang.Value;
            }
            else
            {
                language = request.UserLanguages[0].Split(';')[0];
            }

            var ci = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            base.OnActionExecuting(filterContext);
        }
    }
}