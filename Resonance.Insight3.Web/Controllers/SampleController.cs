using System.Web.Mvc;

namespace Resonance.Insight3.Web.Controllers
{
    public class SampleController : Controller
    {
        //
        // GET: /Sample/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Application/

        public PartialViewResult Header(string nodeType, string nodeId)
        {
            return PartialView();
        }

        //
        // GET: /Application/

        public PartialViewResult Details()
        {
            return PartialView();
        }
    }
}