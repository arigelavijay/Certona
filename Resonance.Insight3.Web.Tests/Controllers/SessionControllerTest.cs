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
    public class SessionControllerTest
    {

        [TestMethod]
        public void WarningTest_View()
        {

            #region Setup Test Data

            var controller = new SessionController();

            #endregion


            #region Execute Test

            var result = controller.Warning() as PartialViewResult;

            #endregion


            #region Evaluate Results

            Assert.AreEqual("Warning", result.ViewName, "Must return Warning partial view");

            #endregion

        }

        [TestMethod]
        public void TimeoutTest_View()
        {

            #region Setup Test Data

            var controller = new SessionController();

            #endregion


            #region Execute Test

            var result = controller.Timeout() as PartialViewResult;

            #endregion


            #region Evaluate Results

            Assert.AreEqual("Timeout", result.ViewName, "Must return Timeout partial view");

            #endregion

        }

        [TestMethod]
        public void TimeoutTest_WithCredentialsData()
        {

            #region Valid user with user details

            #region Setup Test Data

            var controller = new SessionController();
            LoginViewModel model = new LoginViewModel { Account = "ValidAccountID", UserName = "ValidUserName", Password = "ValidPassword" };
            UserDTO user = new UserDTO() { Email = "Test@gmail.com", Name = "Test", UserID = "TestUserID" };

            var response = new AuthenticateUserResponse() { Success = true, User = user };

            Mock<ICertonaService> applicationMock = new Mock<ICertonaService>();
            controller.ForesightService = applicationMock.Object;
            applicationMock.Setup(p => p.AuthenticateUser(It.IsAny<AuthenticateUserRequest>())).Returns(response);

            Mock<IFormsAuthentication> formsAuthenticationMock = new Mock<IFormsAuthentication>();
            controller.FormsAuth = formsAuthenticationMock.Object;
            formsAuthenticationMock.Setup(p => p.SetAuthCookie(It.IsAny<string>(), It.IsAny<CertonaCredential>(), It.IsAny<bool>()));
            #endregion


            #region Execute Test



            var result = controller.Timeout("ValidAccountID", "ValidUserName", "ValidPassword");

            #endregion


            #region Evaluate Results

            Assert.IsTrue(controller.ModelState.IsValid, "ModelState must be valid for valid credentials");
            // It should redirect to home page
            JsonResult jsonResult = (JsonResult)result;
            Assert.IsInstanceOfType(result, typeof(JsonResult), "Should return json result");
            Assert.AreEqual("Success", jsonResult.Data,
                "Default Login with authenticated user set should return success data");

            #endregion


            #endregion
        }

        [TestMethod]
        public void TimeoutTest_WithInvalidCredentialsData()
        {

            #region Invalid user with user details

            #region Setup Test Data
            var controller = new SessionController();
            LoginViewModel model = new LoginViewModel { Account = "InValidAccountID", UserName = "InValidUserName", Password = "InValidPassword" };


            ErrorResult[] errors = new ErrorResult[] { new ErrorResult { Description = "Invalid credentials" } };
            var response = new AuthenticateUserResponse() { Success = false, Errors = errors };

            Mock<ICertonaService> authenticationMock = new Mock<ICertonaService>();
            controller.ForesightService = authenticationMock.Object;
            authenticationMock.Setup(p => p.AuthenticateUser(It.IsAny<AuthenticateUserRequest>())).Returns(response);

            Mock<IFormsAuthentication> formsAuthenticationMock = new Mock<IFormsAuthentication>();
            controller.FormsAuth = formsAuthenticationMock.Object;
            formsAuthenticationMock.Setup(p => p.SetAuthCookie(It.IsAny<string>(), It.IsAny<CertonaCredential>(), It.IsAny<bool>()));

            #endregion


            #region Execute Test


            var result = controller.Timeout("InValidAccountID", "InValidUserName", "InValidPassword");

            #endregion


            #region Evaluate Results
            Assert.IsFalse(controller.ModelState.IsValid,
                "ModelState must be invalid for invalid credentials");
            JsonResult jsonResult = (JsonResult)result;
            Assert.IsInstanceOfType(result, typeof(JsonResult), "Should return json result");
            Assert.AreEqual("Invalid credentials\r\n", jsonResult.Data,
                "Invalid credentials should return error message 'Invalid credentials' ");

            #endregion

            #endregion

        }
    }
}
