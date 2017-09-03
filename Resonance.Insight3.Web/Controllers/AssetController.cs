using System.Collections.Generic;
using System.Web.Mvc;
using Resonance.Insight3.Web.Models;
using Resonance.Insight3.Web.ViewModels.Asset;

namespace Resonance.Insight3.Web.Controllers
{
    public class AssetController : Controller
    {
        public PartialViewResult Header(string id, string catalogid)
        {
            var model = AssetModel.GetCatalogAssetDetails(catalogid, id);
            return PartialView(model);
        }

        public PartialViewResult ViewItemDetails()
        {
            return PartialView();
        }

        public PartialViewResult ViewDetails(string id, string catalogid)
        {
            var model = AssetModel.GetCatalogAssetDetailsWithMapping(catalogid, id);
            return PartialView(model);
        }

        public PartialViewResult ViewEnumerations(AssetEnumeration model)
        {
            model.Values.Sort();
            return PartialView(model);
        }
    }
}
