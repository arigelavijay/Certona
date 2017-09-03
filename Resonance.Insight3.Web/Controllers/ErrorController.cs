using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Elmah;

namespace Resonance.Insight3.Web.Controllers
{
    public class ErrorController : Controller
    {
        // GET: /Error/General
        public ActionResult General()
        {
            return View();
        }


        // GET: /Error/Http404
        public ActionResult Http404()
        {
            return View();
        }

        // GET: /Error/Http403
        public ActionResult Http403()
        {
            return View();
        }

        [HttpPost]
        public void LogJavaScriptError(string message)
        {
            ErrorSignal.FromCurrentContext().Raise(new Resonance.Insight3.Web.Models.NavigationModel.JavaScriptErrorException(message));
        }
    }
}
