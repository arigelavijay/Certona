using System;
using System.ServiceModel;
using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.FormAuthentication;
using Resonance.Insight3.Web.Helpers;
using Resonance.Insight3.Web.ViewModels;

namespace Resonance.Insight3.Web.Models
{
    public class AuthenticationModel : ModelBase
    {
        /// <summary>
        ///     Creates new instance of FormsAuthenticationWrapper
        /// </summary>
        private static IFormsAuthentication _formsAuth;

        public static IFormsAuthentication FormsAuth
        {
            set { _formsAuth = value; } // Dependancy Injection - Called from test framework only 
            get { return _formsAuth ?? (_formsAuth = new FormsAuthenticationWrapper()); }
        }

        public static string AuthenticateUser(LoginViewModel model)
        {
            try
            {
                using(_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        var user = new UserDTO
                        {
                            UserID = model.UserName,
                            Password = Encryption.GetMd5Sum(model.Password)
                        };
                        var request = new AuthenticateUserRequest { User = user };

                        AuthenticateUserResponse response = _certonaService.AuthenticateUser(request);
                        if (!response.Success)
                        {
                            return Utilities.ParseServiceErrors(response.Errors);
                        }
                        if (response.User == null || response.User.UserID == null)
                        {
                            return "Invalid user account.";
                        }

                        var userDetailsRequest = new GetUserAccountApplicationFeaturesRequest { User = response.User };
                        var userDetailsResponse = _certonaService.GetUserAccountApplicationFeatures(userDetailsRequest);
                        if (userDetailsResponse.Success)
                        {
                            response.User.Accounts = userDetailsResponse.Accounts;
                        }

                        FormsAuth.SetAuthCookie(response.User, false);

                        ReportingStartDate = DateTime.Now.AddDays(-30);
                        ReportingEndDate = DateTime.Now;
                    }
                    catch (TimeoutException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                    catch (CommunicationException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                    
                }
            }
            catch(Exception ex)
            {
                
            }

            return "";
        }
    }
}