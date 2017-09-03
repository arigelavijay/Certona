using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Resonance.Insight.Web;
using Resonance.Insight.Web.Controllers;
using Resonance.Insight.Web.CertonaService;
using System.Web.Routing;
using System.Security.Principal;
using Moq;
using Resonance.Insight.Web.Models;

namespace Resonance.Insight.Web.Tests.Controllers
{
    [TestClass]
    public class NavigationControllerTest
    {

        [TestMethod]
        public void SetSelectedNodeTest()
        {

            #region Setup Test Data

            var controller = new NavigationController();

            #endregion


            #region Execute Test

            controller.SetSelectedNode("1", "TestNodeType", 1);

            #endregion


            #region Evaluate Results

            #endregion

        }

        [TestMethod]
        public void SetAccountExplorerNodeStateTest()
        {

            #region Setup Test Data

            var controller = new NavigationController();

            #endregion


            #region Execute Test

            controller.SetAccountExplorerNodeState("1", "TestNodeType", "TestNodeName", 1, true);

            #endregion


            #region Evaluate Results

            #endregion

        }

        [TestMethod]
        public void SetAssetExplorerNodeStateTest()
        {

            #region Setup Test Data

            var controller = new NavigationController();

            #endregion


            #region Execute Test

            controller.SetAssetExplorerNodeState("1", "TestNodeType", "TestNodeName", 1, true);

            #endregion


            #region Evaluate Results

            #endregion

        }

        [TestMethod]
        public void SetPanelBarWidthTest()
        {

            #region Setup Test Data

            var controller = new NavigationController();

            #endregion


            #region Execute Test

            controller.SetPanelBarWidth(100);

            #endregion


            #region Evaluate Results

            #endregion

        }

        [TestMethod]
        public void SetPanelBarStatesTest()
        {

            #region Setup Test Data

            var controller = new NavigationController();

            #endregion


            #region Execute Test

            controller.SetPanelBarStates("TestPanelBarName");

            #endregion


            #region Evaluate Results

            #endregion

        }
    }
}
