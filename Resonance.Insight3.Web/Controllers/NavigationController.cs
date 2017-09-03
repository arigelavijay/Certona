using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.FormAuthentication;
using Resonance.Insight3.Web.Helpers;
using Resonance.Insight3.Web.Models;

namespace Resonance.Insight3.Web.Controllers
{
    public class NavigationController : Controller
    {
        /// <summary>
        ///     Creates new instance of the service
        /// </summary>
        private ICertonaService foresightService;

        public ICertonaService ForesightService
        {
            set { foresightService = value; } // Dependancy Injection - Called from test framework only 
            get
            {
                if (foresightService == null)
                    foresightService = new CertonaServiceClient();
                return foresightService;
            }
        }

        /// <summary>
        ///     Set Selected Node
        /// </summary>
        /// <param name="nodeID">nodeID</param>
        /// <param name="nodeType">nodeType</param>
        /// <param name="classificationID">classificationID</param>
        /// <returns>Returns nothing.</return>
        [HttpPost]
        public void SetSelectedNode(string nodeID, string nodeType, int? classificationID)
        {
            NavigationModel.SetSelectedNode(nodeID, nodeType, classificationID);
        }

        /// <summary>
        ///     Set Account Explorer Node State
        /// </summary>
        /// <param name="nodeID">nodeID</param>
        /// <param name="nodeType">nodeType</param>
        /// <param name="nodeName">nodeName</param>
        /// <param name="classificationID">classificationID</param>
        /// <param name="expand">expand</param>
        /// <returns>Returns nothing.</returns>
        [HttpPost]
        public void SetAccountExplorerNodeState(
            string nodeID,
            string nodeType,
            string nodeName,
            int? classificationID,
            bool expand)
        {
            NavigationModel.SetAccountExplorerNodeState(nodeID, nodeType, nodeName, classificationID, expand);
        }

        /// <summary>
        ///     Set Asset Explorer Node State
        /// </summary>
        /// <param name="nodeID">nodeID</param>
        /// <param name="nodeType">nodeType</param>
        /// <param name="nodeName">nodeName</param>
        /// <param name="classificationID">classificationID</param>
        /// <param name="expand">expand</param>
        /// <returns>Returns nothing.</returns>
        [HttpPost]
        public void SetAssetExplorerNodeState(string nodeID, string nodeType, string nodeName, bool expand)
        {
            NavigationModel.SetAssetExplorerNodeState(nodeID, nodeType, nodeName, expand);
        }

        /// <summary>
        ///     Set PanelBar Width
        /// </summary>
        /// <param name="width">width</param>
        /// <returns>Returns nothing.</returns>
        [HttpPost]
        public void SetPanelBarWidth(decimal width)
        {
            NavigationModel.SetPanelBarWidth(width);
        }

        public JsonResult GetAccountExplorerStatuses()
        {
            var accountExplorerStatusRequest = new GetAccountExplorerStatusRequest { User = FormsAuthenticationWrapper.User };
            var accountExplorerStatusResponse = ForesightService.GetAccountExplorerStatusList(accountExplorerStatusRequest);
            return Json(accountExplorerStatusResponse.StatusValues.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPanelBarStates()
        {
            var panelBarStatesRequest = new GetPanelBarStatesRequest { User = FormsAuthenticationWrapper.User };
            var panelBarStatesResponse = ForesightService.GetPanelBarStates(panelBarStatesRequest);
            var panelBarStates = panelBarStatesResponse.ExpandedPanelBars.Select(pbs => pbs.PanelBarID);
            return Json(panelBarStates, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Set PanelBar States
        /// </summary>
        /// <param name="panelBarName">panelBarName</param>
        /// <returns>Returns nothing.</returns>
        [HttpPost]
        public void SetPanelBarStates(int panelBarId)
        {
            NavigationModel.SetPanelBarStates(panelBarId);
        }

        public JsonResult GetReportTreeItems()
        {
            return Json(NavigationModel.GetReportTreeItems(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccountTreeItems()
        {
            return Json(NavigationModel.GetAccountTreeItems(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssetTreeItems()
        {
            return Json(NavigationModel.GetAssetTreeItems(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetToolTreeItems()
        {
            return Json(NavigationModel.GetToolTreeItems(), JsonRequestBehavior.AllowGet);
        }
    }
}