using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Resonance.Insight3.Web.CertonaService;

namespace Resonance.Insight3.Web.Helpers
{
    public class Utilities
    {
        /// <summary>
        ///     Converts error results to a string.
        /// </summary>
        /// <param name="result">ErrorResult array</param>
        /// <returns>error message which will be displayed to the user</returns>
        public static string ParseServiceErrors(List<ErrorResult> result)
        {
            if (result == null || result.Count == 0)
            {
                return "Service error, Please try again!";
            }
            var errorDescription = new StringBuilder();
            foreach (var er in result)
            {
                switch(er.ServiceResultMessage)
                {
                    case ServiceResultMessage.LoginAccountDoesNotExist:
                        errorDescription.Append(Resource.LoginAccountDoesNotExist);
                        break;
                    case ServiceResultMessage.LoginAttemptsExceeded:
                        errorDescription.Append(Resource.LoginAttemptsExceeded);
                        break;
                    case ServiceResultMessage.LoginInvalidCredentials:
                        errorDescription.Append(Resource.LoginInvalidCredentials);
                        break;
                    case ServiceResultMessage.LoginInvalidCredentialsWithCount:
                        errorDescription.Append(string.Format(Resource.LoginInvalidCredentialsWithCount, er.RemainingLoginAttempts));
                        break;
                }
                errorDescription.AppendLine();
                errorDescription.AppendLine();
                errorDescription.Append(er.Description);
                errorDescription.AppendLine();
                errorDescription.AppendLine();
            }
            return errorDescription.ToString();
        }

        public static IEnumerable Errors(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return modelState.ToDictionary(kvp => kvp.Key,
                                               kvp => kvp.Value
                                                         .Errors
                                                         .Select(e => e.ErrorMessage)
                                                         .ToArray())
                                 .Where(m => m.Value.Count() > 0);
            }
            return null;
        }
    }
}