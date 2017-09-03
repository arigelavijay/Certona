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
    public class ContentControllerTest
    {

        [TestMethod]
        public void GetContentTest_View()
        {

            #region Setup Test Data

            var controller = new ContentController();
            #endregion


            #region Execute Test

            var result = controller.GetContent();

            #endregion


            #region Evaluate Results

            JsonResult jsonResult = (JsonResult)result;
            Assert.IsInstanceOfType(result, typeof(JsonResult), "Should return json result");

            #endregion

        }
    }
}
