using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using Resonance.Insight3.Web.Models;
using Resonance.Insight3.Web.ViewModels.Variant;
using Resonance.Insight3.Web.ViewModels.Rule;
using VariantModel = Resonance.Insight3.Web.Models.VariantModel;

namespace Resonance.Insight3.Web.Controllers
{
    public class VariantController : Controller
    {
        public PartialViewResult ViewModifiers(int id)
        {
            var model = VariantModel.GetVariantBoosts(id);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult UpdateVariantModifiers(string variantId, int pool, int profile, List<VariantBiases> biases, List<ViewModels.Variant.VariantModel> variants)
        {
            var model = VariantModel.UpdateVariantModifiers(variantId, pool, profile, biases, variants);
            return Json(model);
        }

        [HttpGet]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public PartialViewResult ManageModifiers(int id, int cpsId)
        {
            var model = VariantModel.GetVariantBoosts(id);
            ViewBag.cpsId = cpsId;
            return PartialView(model);
        }

        public PartialViewResult ViewDetails(int id)
        {
            var model = VariantModel.GetVariantDetails(id);
            return PartialView(model);
        }

        public PartialViewResult ViewRules(int id)
        {
            var model = VariantModel.GetVariantRules(id);
            return PartialView(model);
        }

        public PartialViewResult Header(int id)
        {
            var model = VariantModel.GetVariantDetails(id);
            return PartialView(model);
        }

        public PartialViewResult ViewTrends(int id)
        {
            var model = ReportingModel.GetTrendsVariantData(id);
            return PartialView("~/Views/Reporting/ViewTrends.cshtml", model);
        }

        [HttpGet]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public PartialViewResult ManageDetails(int id, int cpsId)
        {
            var model = VariantModel.GetVariantDetails(id);
            ViewBag.cpsId = cpsId;
            return PartialView(model);
        }

        [HttpPost]
        public void ManageDetails(VariantDetails details)
        {
            // Save the variant Details
            VariantModel.UpdateVariantDetails(details);
        }

        [HttpPost]
        public void CreateVariant(VariantDetails details)
        {
            /********************************************************
             *      This method is used for the Modal Dialog POC    *
             ********************************************************/
            var Name = details.Name;
            var Description = details.Description;
            var Priority = details.Priority;
            var NumberOfAssets = details.NumberOfAssets;
        }

        [HttpPost]
        public JsonResult UpdateVariantAction(string variantList, string actionStatus, string experienceID)
        {
            switch (actionStatus)
            {
                case "Move Up":
                case "Move Down":
                    VariantModel.UpdateVariantListPriority(variantList, actionStatus);
                    break;
                default:
                    VariantModel.UpdateVariantStatus(variantList, actionStatus);
                    break;
            }

            // pass list back so we can rebind grid
            var m = ExperienceModel.GetExperienceStrategies(int.Parse(experienceID));
            return Json(new { strategies = m }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VariantActionList()
        {
            var model = ModelBase.GetStatusList();
            model.Add("Move Up");
            model.Add("Move Down");
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public PartialViewResult CreateRules(int NodeId, string updateTargetId)
        {
            var rm = new RuleViewModel() { VariantID = NodeId };

            ViewBag.UpdateTargetId = updateTargetId;
            return PartialView(rm);
        }

        [HttpPost]
        public JsonResult CreateRules(int variantID, int ruleID, string rulename)
        {
            if (ruleID <= 0)   // create
            {
                ruleID = RuleModel.CreateRule(variantID, rulename);
            }
            else
            {
                // TODO Update
            }

            return Json(new { id = ruleID, name = rulename });
        }

        [HttpPost]
        public JsonResult DeleteRules(int ruleID)
        {
            bool isDelete = RuleModel.DeleteRule(ruleID);
            if (isDelete)
                return Json("success");
            else
                return Json("Error");
        }

        [HttpGet]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public PartialViewResult AddFilter(int variantID, string ruleName, int ruleID)
        {
            var vm = new ExpressionViewModel() { VariantID = variantID, RuleName = ruleName, RuleID = ruleID };
            return PartialView(vm);
        }

        [HttpGet]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public PartialViewResult SelectAttribute(int ruleID)
        {
            ViewBag.ruleID = ruleID;
            return PartialView();
        }

        public JsonResult GetRuleOperators(string attributeType)
        {
            return Json(RuleModel.GetRuleOperators(attributeType), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetControlSet(string attributeType, string operatorType)
        {
            return Json(RuleModel.GetControlSet(attributeType, operatorType), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCatalogMappingFields(string ruleId)
        {
            return Json(RuleModel.GetCatalogMappingFields(System.Convert.ToInt32(ruleId)), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveExpression(ExpressionViewModel expression)
        {
            int newId = RuleModel.SaveExpression(expression);
            return Json(new { id = newId });
            //return PartialView("ViewRules"); 
        }
    }
}
