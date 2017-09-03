using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Resonance.Insight3.Web.Models;
using Resonance.Insight3.Web.ViewModels.Catalog;

namespace Resonance.Insight3.Web.Controllers
{
    public class CatalogController : Controller
    {
        public PartialViewResult Header(string id)
        {
            var vm = CatalogModel.GetCatalogHeader(id);
            return PartialView(vm);
        } 

        public PartialViewResult ViewDetails(string id)
        {
            var model = CatalogModel.GetCatalogDetails(id);
            return PartialView(model);
        }

        [HttpGet]
        public PartialViewResult ManageDetails(string id)
        {
            var model = CatalogModel.GetCatalogDetails(id);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult ManageDetails(CatalogDetails vm)
        {
            try
            {
                //Failure Response
                //return Json(new { Success = false, Message = String.Format("There was an error updating {0}'s details.", vm.Name), Name = "Details" });
                //Success Response
                return Json(new { Success = true, Message = String.Format("Catalog {0} details saved successfully!", vm.Name), Name = "Details" });
            }
            catch (Exception ex)
            {
                return Json(new {Success = false, ex.Message});
            }
        }

        public PartialViewResult ViewAssets(string id)
        {
            var model = CatalogModel.GetCatalogMappingsForAccountItemIDAndDescription(id);
            model.CatalogID = id;
            return PartialView(model);
        }

        public ActionResult GetAssets([DataSourceRequest] DataSourceRequest request, string catalogId)
        {
            CatalogModel.BindDataSourceRequest(Request, ref request);

            var response = CatalogModel.GetCatalogItems(request, catalogId);
            var ret = new
                {
                    Data = response.Items,
                    response.Total
                };
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
    }
}
