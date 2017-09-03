using System.Web.Mvc;
using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.Models;
using Resonance.Insight3.Web.ViewModels;

namespace Resonance.Insight3.Web.Controllers
{
    public class ContentController : Controller
    {
        /// <summary>
        ///     Get content for content panel
        /// </summary>
        /// <returns>Return json object</returns>
        [HttpGet]
        public ActionResult GetContent()
        {
            string content = string.Empty;
            // Get the content for content panel
            return Json(content);
        }

        [HttpGet]
        public void SetContentPanelSectionState(int contentPanelSectionID)
        {
            ContentModel.SetContentPanelSectionState(contentPanelSectionID);
        }

        public PartialViewResult GetContentPanel(NodeDTO node)
        {
            IViewModel vm = new ContentViewModel
                {
                    NodeID = node.NodeID,
                    NodeType = node.NodeType.ToString().Replace("_", ""),
                    ApplicationID = node.ApplicationID,
                    CatalogID = node.CatalogID
                };

            var panel = ContentModel.GetContentPanel((int)node.NodeType);

            vm.ContentPanelSections = panel.ContentPanelSections;

            return PartialView(panel.PartialViewName, vm);
        }

        public PartialViewResult GetAccountChanged()
        {
            return PartialView("_AccountChanged");
        }

        public ViewResult Blank()
        {
            return View();
        }
    }
}