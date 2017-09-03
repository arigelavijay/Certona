using System;
using System.Web;
using Resonance.Insight3.Web.CertonaService;
using System.Collections.Generic;
using System.ServiceModel;
using Resonance.Insight3.Web.FormAuthentication;
using System.Linq;

namespace Resonance.Insight3.Web.Models
{
    public class ModelBase
    {
        public static CertonaServiceClient _certonaService;

        public static DateTime ReportingStartDate
        {
            get
            {
                if(HttpContext.Current.Session["Reporting.StartDate"] == null)
                {
                    HttpContext.Current.Session["Reporting.StartDate"] = DateTime.Now.AddDays(-30).Date;
                }
                return (DateTime)HttpContext.Current.Session["Reporting.StartDate"];
            }
            set
            {
                HttpContext.Current.Session["Reporting.StartDate"] = value.Date;
            }
        }

        public static DateTime ReportingEndDate
        {
            get
            {
                if (HttpContext.Current.Session["Reporting.EndDate"] == null)
                {
                    HttpContext.Current.Session["Reporting.EndDate"] = DateTime.Now.Date;
                }
                return (DateTime)HttpContext.Current.Session["Reporting.EndDate"];
            }
            set
            {
                HttpContext.Current.Session["Reporting.EndDate"] = value.Date;
            }
        }

        public static string ReportingAppID
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session["Reporting.AppID"]);
            }
            set
            {
                HttpContext.Current.Session["Reporting.AppID"] = value;
            }
        }

        public static List<string> GetStatusList()
        {
            var details = new List<string>();
            try
            {
                using(_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var statusListRequest = new GetStatusListRequest();
                        var statusListResponse = _certonaService.GetStatusList(statusListRequest);

                        if (statusListResponse.Success && statusListResponse.Statuses != null)
                        {
                            details = statusListResponse.Statuses.Select(s => s.Status).ToList();
                        }
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
            catch (Exception ex)
            {
                throw;
            }

            return details;
        }
    }
}