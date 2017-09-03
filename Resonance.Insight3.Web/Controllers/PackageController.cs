using System.Web.Mvc;
using Resonance.Insight3.Web.Models;

namespace Resonance.Insight3.Web.Controllers
{
    public class PackageController : Controller
    {
        public PartialViewResult Header(int id)
        {
            var model = PackageModel.GetPackageDetails(id);
            return PartialView(model);
        }

        public PartialViewResult ViewDetails(int id)
        {
            var model = PackageModel.GetPackageDetails(id);
            return PartialView(model);
        }

        public PartialViewResult ViewContainers(int id)
        {
            var model = PackageModel.GetPackageSchemes(id);
            return PartialView(model);
        }

        public PartialViewResult ViewTrends(int id)
        {                  
            var model = ReportingModel.GetTrendsPackageData(id);
            return PartialView("~/Views/Reporting/ViewTrends.cshtml", model);
        }
    }
}
