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
    public class HomeControllerTest
    {

        [TestMethod]
        public void HomeTest_View()
        {

            #region Setup Test Data

            var controller = new HomeController();
            GetMockedObject(controller);
            #endregion


            #region Execute Test

            var result = controller.Home() as ViewResult;

            #endregion


            #region Evaluate Results

            Assert.AreEqual("Home", result.ViewName, "Must return Home view");

            #endregion

        }

        [TestMethod]
        public void NavigationPanelTest_View()
        {

            #region Setup Test Data

            var controller = new HomeController();
            GetMockedObject(controller);

            #endregion


            #region Execute Test

            var result = controller.NavigationPanel("") as PartialViewResult;

            #endregion


            #region Evaluate Results

            Assert.AreEqual("_NavigationPanel", result.ViewName, "Must return NavigationPanel view");

            #endregion

        }

        [TestMethod]
        public void ContentPanelTest_View()
        {

            #region Setup Test Data

            var controller = new HomeController();

            #endregion


            #region Execute Test

            var result = controller.ContentPanel() as PartialViewResult;

            #endregion


            #region Evaluate Results

            Assert.AreEqual("_ContentPanel", result.ViewName, "Must return ContentPanel view");

            #endregion

        }

        private void GetMockedObject(HomeController controller)
        {
            AccountDTO[] accountDTO = new AccountDTO[] { new AccountDTO { AccountID = "TestAccountID", AccountName = "Test Account Name" } };
            ReportDTO[] reportDTO = new ReportDTO[] { new ReportDTO { ReportID = 1, ReportName = "Test Report Name" } };
            NodeDTO[] nodeDTO = new NodeDTO[] { new NodeDTO { NodeID = "TestNodeID", DisplayValue = "Test Node Name" } };

            Mock<ICertonaService> applicationMock = new Mock<ICertonaService>();
            controller.ForesightService = applicationMock.Object;

            GetAccountsByUserResponse accountsByUserResponse = new GetAccountsByUserResponse { Success = true, Accounts = accountDTO };
            GetPanelBarWidthResponse panelBarWidthResponse = new GetPanelBarWidthResponse { Width = 100 };
            GetReportsListResponse reportsListResponse = new GetReportsListResponse { Reports = reportDTO };
            GetAccountExplorerTreeResponse accountExplorerTreeResponse = new GetAccountExplorerTreeResponse { Nodes = nodeDTO };

            applicationMock.Setup(p => p.GetAccountsByUser(It.IsAny<GetAccountsByUserRequest>())).Returns(accountsByUserResponse);
            applicationMock.Setup(p => p.GetPanelBarWidth(It.IsAny<GetPanelBarWidthRequest>())).Returns(panelBarWidthResponse);
            applicationMock.Setup(p => p.GetReportsList(It.IsAny<GetReportsListRequest>())).Returns(reportsListResponse);
            applicationMock.Setup(p => p.GetAccountExplorerTree(It.IsAny<GetAccountExplorerTreeRequest>())).Returns(accountExplorerTreeResponse);
        }

        [TestMethod]
        public void SetAccountExplorerStatusTest()
        {

            #region Setup Test Data

            var controller = new HomeController();

            #endregion


            #region Execute Test

            controller.SetAccountExplorerStatus("Active");

            #endregion


            #region Evaluate Results

            #endregion

        }

        [TestMethod]
        public void GetAccountTreeviewTest_View()
        {

            #region Setup Test Data

            var controller = new HomeController();

            #endregion


            #region Execute Test

            var result = controller.GetAccountTreeview() as PartialViewResult;

            #endregion


            #region Evaluate Results

            Assert.AreEqual("_AccountTreeView", result.ViewName, "Must return account tree view");

            #endregion

        }
    }
}
