using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Resonance.Insight3.Web.Models;
using Resonance.Insight3.Web.ViewModels.Models;

namespace Resonance.Insight3.Web.Controllers
{
    public class ModelsController : Controller
    {
        public PartialViewResult Header(int id)
        {
            var vm = ModelsModel.GetModel(id);
            return PartialView(vm);
        }

        public PartialViewResult ViewModelData(int id)
        {
            var vm = ModelsModel.GetModel(id);
            return PartialView(vm);
        }

        public PartialViewResult ViewModelVisualization(int id)
        {
            var vm = ModelsModel.GetModel(id);
            return PartialView(vm);
        }
        
        public ActionResult GetSegmentItems(int? ModelID, int? SegmentID, string SegmentName)
        {
            var viewModel = new SegmentItemsGridViewModel();
            if (ModelID.HasValue && SegmentID.HasValue)
            {
                viewModel = ModelsModel.GetSegmentItems(ModelID.Value, SegmentID.Value);

                viewModel.SegmentName = SegmentName;
                viewModel.ModelID = ModelID;
            }

            return PartialView("SegmentItemsGridPartialView", viewModel);
        }

        public ActionResult ModelsGridSelect([DataSourceRequest]DataSourceRequest request, int ModelID)
        {
            ModelDataViewModel viewModel = ModelsModel.GetModel(ModelID);

            return Json(viewModel.GridData.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SegmentItemsGridSelect([DataSourceRequest]DataSourceRequest request, int? ModelID, int? SegmentID)
        {
            if (ModelID.HasValue && SegmentID.HasValue)
            {
                SegmentItemsGridViewModel viewModel = ModelsModel.GetSegmentItems(ModelID.Value, SegmentID.Value);
                viewModel.ModelID = ModelID;

                return Json(viewModel.SegmentItems.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            
            return null;
        }
    }
}
