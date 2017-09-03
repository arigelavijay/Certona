using System.Web.Mvc;
using System.Web.UI;
using Resonance.Insight3.Web.Models;
using Resonance.Insight3.Web.ViewModels.Experience;
using Resonance.Insight3.Web.ViewModels.Scheme;

namespace Resonance.Insight3.Web.Controllers
{
    public class SchemeController : Controller
    {
        public PartialViewResult Header(int id)
        {
            var model = SchemeModel.GetSchemeDetails(id);
            return PartialView(model);
        }

        public PartialViewResult ViewVisitorHistorySettings(int id)
        {
            var model = SchemeModel.GetSchemeContext(id);
            return PartialView(model);
        }

        public PartialViewResult ViewDetails(int id)
        {
            var model = SchemeModel.GetSchemeDetails(id);
            return PartialView(model);
        }

        public PartialViewResult ViewExperiences(int id)
        {
            var vm = SchemeModel.GetSchemeExperiences(id);
            return PartialView(vm);
        }

        public PartialViewResult ViewTrends(int id)
        {
            var model = ReportingModel.GetTrendsSchemeData(id);
            return PartialView("~/Views/Reporting/ViewTrends.cshtml", model);
        }

        public PartialViewResult ManageExperiences(int id, int cpsId)
        {
            var vm = SchemeModel.GetSchemeExperiences(id);
            ViewBag.cpsId = cpsId;
            return PartialView(vm);
        }

        [HttpGet]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public PartialViewResult CreateExperiences(int NodeId, string updateTargetId)
        {
            //var vm = //new SchemeExperience() { SchemeID = NodeId };
            var vm = new ExperienceNodeViewModel() { SchemeID = NodeId };

            ViewBag.UpdateTargetId = updateTargetId;
            return PartialView(vm);
        }


        public PartialViewResult CreateNewExperience(ExperienceNodeViewModel experience)
        {
            int newId = ExperienceModel.CreateExperience(experience);

            var vm = SchemeModel.GetSchemeExperiences(experience.SchemeID);
            vm.CreatedExperienceId = newId;
            vm.CreatedExperienceName = experience.Name;
            return PartialView("ViewExperiences", vm);
        }

        //[HttpPost]
        //public JsonResult SaveNewExperience(ExperienceNodeViewModel model)
        //{
        //    int newExperience = 9999; // ExperienceModel.CreateExperience(model);
        //    
        //    // pass list back so we can rebind
        //    var m = SchemeModel.GetSchemeExperiences(model.SchemeID);
        //
        //    return Json(new { id = newExperience, experiences = m.Experiences });
        //}
    }
}
