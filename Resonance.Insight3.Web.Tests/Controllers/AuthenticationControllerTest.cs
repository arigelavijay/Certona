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
    public class AuthenticationControllerTest
    {

        [TestMethod]
        public void LoginTest_View()
        {

            #region Setup Test Data

            var controller = new AuthenticationController();

            #endregion


            #region Execute Test

            var result = controller.Login() as ViewResult;

            #endregion


            #region Evaluate Results

            Assert.AreEqual("Login", result.ViewName, "Must return Login view");

            #endregion

        }

        [TestMethod]
        public void LoginTest_WithCredentialsData()
        {

            #region Valid user with user details

            #region Setup Test Data

            var controller = new AuthenticationController();
            LoginViewModel model = new LoginViewModel { Account = "ValidAccountID", UserName = "ValidUserName", Password = "ValidPassword" };
            UserDTO user = new UserDTO() { Email = "Test@gmail.com", Name = "Test", UserID = "TestUserID" };

            var response = new AuthenticateUserResponse() { Success = true, User = user };

            Mock<ICertonaService> applicationMock = new Mock<ICertonaService>();
            controller.ForesightService = applicationMock.Object;
            applicationMock.Setup(p => p.AuthenticateUser(It.IsAny<AuthenticateUserRequest>())).Returns(response);

            Mock<IFormsAuthentication> formsAuthenticationMock = new Mock<IFormsAuthentication>();
            controller.FormsAuth = formsAuthenticationMock.Object;
            formsAuthenticationMock.Setup(p => p.SetAuthCookie(It.IsAny < string>(), It.IsAny<CertonaCredential>(), It.IsAny<bool>()));
            #endregion


            #region Execute Test

           

            var result = controller.Login(model);

            #endregion


            #region Evaluate Results

            Assert.IsTrue(controller.ModelState.IsValid, "ModelState must be valid for valid credentials");
            // It should redirect to home page
            RedirectResult redirectResult = (RedirectResult)result;
            Assert.AreEqual(@"~/Home", redirectResult.Url,
                "Default Login with authenticated user set should be redirected to home page");

            #endregion


            #endregion
        }

        [TestMethod]
        public void LoginTest_WithInvalidCredentialsData() {

            #region Invalid user with user details

            #region Setup Test Data
            var controller = new AuthenticationController();
            LoginViewModel model = new LoginViewModel { Account = "InValidAccountID", UserName = "InValidUserName", Password = "InValidPassword" };

            
            ErrorResult[] errors = new ErrorResult[] { new ErrorResult { Description = "Invalid credentials" } };
            var response = new AuthenticateUserResponse() { Success = false,  Errors = errors };

            Mock<ICertonaService> authenticationMock = new Mock<ICertonaService>();
            controller.ForesightService = authenticationMock.Object;
            authenticationMock.Setup(p => p.AuthenticateUser(It.IsAny<AuthenticateUserRequest>())).Returns(response);

            Mock<IFormsAuthentication> formsAuthenticationMock = new Mock<IFormsAuthentication>();
            controller.FormsAuth = formsAuthenticationMock.Object;
            formsAuthenticationMock.Setup(p => p.SetAuthCookie(It.IsAny<string>(), It.IsAny<CertonaCredential>(), It.IsAny<bool>()));

            #endregion


            #region Execute Test

            
            var result = controller.Login(model);

            #endregion


            #region Evaluate Results
            ViewResult viewResult = (ViewResult)result;
            Assert.IsFalse(controller.ModelState.IsValid,
                "ModelState must be invalid for invalid credentials");
            Assert.AreEqual(response.Success, false, "Response should be false.");
            Assert.AreEqual("Login", viewResult.ViewName, "Must return Login view");
            #endregion

            #endregion

        }

        [TestMethod]
        public void LogoffTest_View()
        {

            #region Setup Test Data

            var controller = new AuthenticationController();

            Mock<IFormsAuthentication> formsAuthenticationMock = new Mock<IFormsAuthentication>();
            controller.FormsAuth = formsAuthenticationMock.Object;
            formsAuthenticationMock.Setup(p => p.SignOut());

            #endregion


            #region Execute Test

            var result = controller.Logoff();

            #endregion


            #region Evaluate Results

            RedirectToRouteResult redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("Login", redirectResult.RouteValues["action"],
                "Logoff should redirect to Login");

            #endregion

        }
    }
}
