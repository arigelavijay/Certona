using System.Web.Mvc;
using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.FormAuthentication;
using Resonance.Insight3.Web.Models;

namespace Resonance.Insight3.Web.Controllers
{
    public class ApplicationController : Controller
    {
        public PartialViewResult Header(string id)
        {
            var model = ApplicationModel.GetApplicationDetailsViewModel(id);
            return PartialView(model);
        }

        public PartialViewResult ViewDetails(string id)
        {
            var model = ApplicationModel.GetApplicationDetailsViewModel(id);
            return PartialView(model);
        }

        public PartialViewResult ViewLocations(string id)
        {
            var model = ApplicationModel.GetApplicationPackagesViewModel(id);
            return PartialView(model);
        }

        public PartialViewResult ViewTrends(string id)
        {
            var model = ReportingModel.GetTrendsApplicationData(id);   
            return PartialView("~/Views/Reporting/ViewTrends.cshtml", model);
        }
        
        public PartialViewResult RemarketingCampaigns(string id)
        {
            var model = ApplicationModel.GetRemarketingCampaignsViewModel(id);
            return PartialView(model);
        }

        public ActionResult StatusList()
        {
            var model = ModelBase.GetStatusList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult FrequencyCapSave(ApplicationFrequencyCapDTO frequencyCap)
        {
            var success = ApplicationModel.SetFrequencyCap(frequencyCap, FormsAuthenticationWrapper.User);
            return Json(new { Success = success });    
        }

        [HttpGet]
        public ActionResult FrequencyCapGet(string applicationId)
        {
            var dto = ApplicationModel.GetFrequencyCap(applicationId, FormsAuthenticationWrapper.User);
            if (dto != null)
            {
                return Json(new {
                                    Enabled = dto.Enabled,
                                    FrequencyCapDays = dto.FrequencyCapDays, 
                                    FrequencyCapEmail = dto.FrequencyCapEmails
                                }, JsonRequestBehavior.AllowGet);    
            }
            return null;            
        }
    }
}