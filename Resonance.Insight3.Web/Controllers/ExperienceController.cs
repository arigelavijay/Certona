using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.Models;
using Resonance.Insight3.Web.ViewModels.Experience;
using Resonance.Insight3.Web.ViewModels.Scheme;
using Resonance.Insight3.Web.ViewModels.Variant;
using VariantModel = Resonance.Insight3.Web.Models.VariantModel;

namespace Resonance.Insight3.Web.Controllers
{
    public class ExperienceController : Controller
    {
        public PartialViewResult Header(int id)
        {
            var model = ExperienceModel.GetExperienceDetails(id);
            return PartialView(model);
        }

        public PartialViewResult ViewModifiers(int id)
        {
            var model = ExperienceModel.GetExperienceBoosts(id);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult UpdateExperienceModifiers(int experienceId, int pool, int profile)
        {
            var model = ExperienceModel.UpdateExperienceModifiers(experienceId, pool, profile);
            return Json(model);
        }

        [HttpGet]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)] 
        public PartialViewResult ManageModifiers(int id, int cpsId)
        {
            var model = ExperienceModel.GetExperienceBoosts(id);
            ViewBag.cpsId = cpsId;
            return PartialView(model);
        }

        public PartialViewResult ViewDecisionPlan()
        {
            return PartialView();
        }

        public PartialViewResult ViewDetails(int id)
        {
            var model = ExperienceModel.GetExperienceDetails(id);
            return PartialView(model);
        }

        [HttpGet]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public PartialViewResult ManageDetails(int id, int cpsId)
        {
            var model = ExperienceModel.GetExperienceDetails(id);
            ViewBag.cpsId = cpsId;
            return PartialView(model);
        }

        [HttpPost]
        public void ManageDetails(ExperienceNodeViewModel details)
        {
            ExperienceModel.UpdateExperienceDetails(details);
        }

        [HttpGet]
        [OutputCache(Location = OutputCacheLocation.None, NoStore=true)]
        public PartialViewResult CreateStrategies(int NodeId, string updateTargetId)
        {
            var vm = new VariantNodeViewModel() { ExperienceID = NodeId };

            ViewBag.UpdateTargetId = updateTargetId;
            return PartialView(vm);
        }

        [HttpPost]
        public PartialViewResult CreateNewStrategy(VariantNodeViewModel variant)
        {
            int newId = VariantModel.CreateVariant(variant);

            var vm = ExperienceModel.GetExperienceDetailsWithRules(variant.ExperienceID);
            vm.CreatedVariantId = newId;
            return PartialView("ViewStrategies", vm);
        }

        public PartialViewResult ViewStrategies(int id)
        {
            var model = ExperienceModel.GetExperienceDetailsWithRules(id);
            return PartialView(model);
        }

        public PartialViewResult ManageStrategies(int id, int cpsId)
        {
            var model = ExperienceModel.GetExperienceDetailsWithRules(id);
            ViewBag.cpsId = cpsId;
            return PartialView(model);
        }

        public PartialViewResult ViewTrends(int id)
        {
            var model = ReportingModel.GetTrendsExperienceData(id);
            return PartialView("~/Views/Reporting/ViewTrends.cshtml", model);
        }

        
        public ActionResult HierarchyBinding_Experiences(int experienceID, [DataSourceRequest] DataSourceRequest request)
        {
            List<ExperienceStrategy> strategies = ExperienceModel.GetExperienceStrategies(experienceID);
            return Json(strategies.ToDataSourceResult(request));
        }
        
        public ActionResult HierarchyBinding_Strategies(int experienceID, int strategyID, [DataSourceRequest] DataSourceRequest request)
        {
            // get the rules based on strategy/variant ID
            var rules = Models.VariantModel.GetVariantRulesForDetailGrid(strategyID);            
            return Json(rules.ToDataSourceResult(request));
        }
        
        //[HttpGet]
        //public JsonResult GetDecisionContent(int experienceID, int strategyID)
        //{
        //    var exp = ExperienceModel.GetExperienceStrategies(experienceID).FirstOrDefault(x => x.StrategyID == strategyID);
        //    var str = VariantModel.GetVariantSlotString(strategyID);

        //    string s1 =
        //        String.Format(
        //            "<div style='float: left; width: 25%;background-color:PaleTurquoise'>No. of Assets Requested: {0}</div>",
        //            exp != null
        //                ? (exp.NumberOfItems.HasValue ? exp.NumberOfItems.Value.ToString() : string.Empty)
        //                : string.Empty);
        //    string s2 = String.Format("<div style='float: left; width: 25%;background-color:PaleTurquoise'>Min. Required:{0}</div>",
        //            exp != null
        //                ? (exp.MinItemsReturned.HasValue ? exp.MinItemsReturned.Value.ToString() : string.Empty)
        //                : string.Empty);
        //    string s3 = String.Format("<div style='float: left; width: 25%;background-color:PaleTurquoise'>Break on Min: {0}</div>",
        //            exp != null
        //                ? (exp.BreakOnMinItemsReturned.HasValue ? (exp.BreakOnMinItemsReturned.Value == true ? "Yes" : "No"): string.Empty)
        //                : String.Empty);            

        //    string s4 = String.Format("<div style='float: left; width: 25%;background-color:PaleTurquoise'>Slots: {0}</div>", str);
            
        //    return Json(String.Format("{0} {1} {2} {3}",s1,s2,s3,s4), JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public PartialViewResult GetDecisionContent(int experienceID, int strategyID)
        {
            var exp = ExperienceModel.GetExperienceStrategies(experienceID).FirstOrDefault(x => x.StrategyID == strategyID);
            var str = Models.VariantModel.GetVariantSlotString(strategyID);

            var model = new ExperienceStrategyDecisionPlan()
                            {
                                NumberOfItems = (exp != null ? exp.NumberOfItems : null),
                                MinItems = (exp != null ? exp.MinItemsReturned : null),
                                BreakOnMinItems = (exp != null ? exp.BreakOnMinItemsReturned : null),
                                SlotInfo = str
                            };

            return PartialView("DecisionPlanFooter",model);
        }

        [HttpPost]
        public JsonResult SetRulesDisplayState(bool showRules)
        {
            var model = new ExperienceNodeViewModel();
            model.ShowRules = showRules;

            return Json("Success");
        }

        [HttpPost]
        public JsonResult SetDecisionPlanDisplayState(bool showDecisionPlan)
        {
            var model = new ExperienceNodeViewModel();
            model.ShowDecisionPlan = showDecisionPlan;

            return Json("Success");
        }

        [AcceptVerbs(HttpVerbs.Post)]        
        public ActionResult ExperienceInline_Update([DataSourceRequest] DataSourceRequest request, SchemeExperience experience)
        {
            ExperienceModel.UpdateExperienceDetails(experience);
           
            // var vm = SchemeModel.GetSchemeExperiences(experience.SchemeID);
            //return Json(vm.Experiences);
           
            return Json(ModelState.ToDataSourceResult());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetExperiences([DataSourceRequest]DataSourceRequest request, int containerID)
        {
            var vm = SchemeModel.GetSchemeExperiences(containerID);
            DataSourceResult result = vm.Experiences.ToDataSourceResult(request);
            return Json(result);
        }

        public MvcHtmlString GetPersonalizationTypeDdl(string catalogID)
        {
            var list = VariantModel.GetRecTypeList(catalogID);
            var ddl = "<select name='MethodID' id='MethodID' style='width: 200px;' data-original-value=''>";
            if (list.Count == 0)
            {
                ddl += "<option value=''>No Personalization Types</option>";
            }
            else
            {
                foreach (var recType in list)
                {
                    ddl += string.Format("<option value='{0}'>{1}</option>", recType.MethodID.ToString(), recType.Name);
                }
            }
            
            ddl += "</select>";

            return MvcHtmlString.Create(ddl);
        }
    }

}
