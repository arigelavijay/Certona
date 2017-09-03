using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web;
using System.Linq;
using System.ServiceModel;
using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.FormAuthentication;
using Resonance.Insight3.Web.ReportService;
using Resonance.Insight3.Web.ViewModels.Reporting;

namespace Resonance.Insight3.Web.Models
{
    public class ReportingModel : ModelBase
    {
        private static string REVENUE_CONTRIBUTION = "Revenue Contribution";

        #region Reports

        public static ReportsViewModel GetReportViewModel()
        {
            var vm = new ReportsViewModel();
            var vmWithApps = GetUserAccountApplicationFeatures();
            vm.ApplicationList = new List<string>();
            //vm.ApplicationList.Add("Please Select:");

            foreach (var app in vmWithApps.ApplicationList)
            {
                vm.ApplicationList.Add(app);
            }
            
            vm.StartDate = ReportingStartDate;
            vm.EndDate = ReportingEndDate;
            vm.AppID = ReportingAppID;
            return vm;
        }

        public static string GetReportDataModel(DateTime startDate,
            DateTime endDate,
            string nodeId,
            string appId,
            //string segmentId,
            //string schemeId,
            string cultureInfo,
            string strategy
            )
        {
            ReportingStartDate = startDate;
            ReportingEndDate = endDate;
            ReportingAppID = appId;

            ParameterValue[] paramval = new ParameterValue[0];
            UserDTO user = FormsAuthenticationWrapper.User;

            var userApplications = (UserDTO) HttpContext.Current.Session["user"];
            // userApplications.Accounts

            var reportPathPrefix = "";
            var reportName = string.Empty;
            // the node id should reference feature enum versus primary key
            
            switch (nodeId)
            {
                case "1":
                case "9004":
                    reportPathPrefix = ConfigurationManager.AppSettings["ReportPrefix"];
                    reportName = "RecommendationDemand";
                    paramval = new ParameterValue[4];
                    paramval[0] = new ParameterValue();
                    paramval[0].Name = "account";
                    paramval[0].Value = user.LastAccountID;
                    paramval[1] = new ParameterValue();
                    paramval[1].Name = "applicationid";
                    paramval[1].Value = appId.ToLower();
                    paramval[2] = new ParameterValue();
                    paramval[2].Name = "startdate";
                    paramval[2].Value = ReportingStartDate.ToShortDateString();
                    paramval[3] = new ParameterValue();
                    paramval[3].Name = "enddate";
                    paramval[3].Value = ReportingEndDate.ToShortDateString();
                    break;

                case "2":
                case "9005":
                    reportPathPrefix = ConfigurationManager.AppSettings["ReportPrefix"];
                    reportName = "RecommendationDetail";
                    paramval = new ParameterValue[4];
                    paramval[0] = new ParameterValue();
                    paramval[0].Name = "account";
                    paramval[0].Value = user.LastAccountID;
                    paramval[1] = new ParameterValue();
                    paramval[1].Name = "app_id";
                    paramval[1].Value = appId.ToLower();
                    paramval[2] = new ParameterValue();
                    paramval[2].Name = "startdate";
                    paramval[2].Value = ReportingStartDate.ToShortDateString();
                    paramval[3] = new ParameterValue();
                    paramval[3].Name = "enddate";
                    paramval[3].Value = ReportingEndDate.ToShortDateString();
                    break;

                case "3":
                case "9006":
                    reportPathPrefix = ConfigurationManager.AppSettings["ReportPrefix"];
                    reportName = "RecommendedItems";
                    paramval = new ParameterValue[4];
                    paramval[0] = new ParameterValue();
                    paramval[0].Name = "Account_ID";
                    paramval[0].Value = user.LastAccountID; // "Nike"
                    paramval[1] = new ParameterValue();
                    paramval[1].Name = "ApplicationID";
                    paramval[1].Value = appId.ToLower(); // "Nike01US"
                    paramval[2] = new ParameterValue();
                    paramval[2].Name = "T_Start";
                    paramval[2].Value = ReportingStartDate.ToShortDateString();
                    //paramval[3] = new ParameterValue();
                    //paramval[3].Name = "Scheme_ID";
                    //paramval[3].Value = schemeId; // "All Schemes"
                    paramval[3] = new ParameterValue();
                    paramval[3].Name = "T_End";
                    paramval[3].Value = ReportingEndDate.ToShortDateString();
                    paramval[4] = new ParameterValue();
                    paramval[4].Name = "Segment";
                    paramval[4].Value = "All Segments";
                    break;

                case "4":
                case "9007":
                    reportPathPrefix = ConfigurationManager.AppSettings["ReportPrefix"];
                    reportName = "StrategyComparison";
                    paramval = new ParameterValue[7];
                    paramval[0] = new ParameterValue();
                    paramval[0].Name = "Account_ID";
                    paramval[0].Value = user.LastAccountID;
                    paramval[1] = new ParameterValue();
                    paramval[1].Name = "ApplicationID";
                    paramval[1].Value = appId.ToLower();
                    paramval[2] = new ParameterValue();
                    paramval[2].Name = "T_Start";
                    paramval[2].Value = ReportingStartDate.ToShortDateString();
                    //paramval[3] = new ParameterValue();
                    //paramval[3].Name = "Scheme_ID";
                    //paramval[3].Value = schemeId; // "All Schemes"
                    paramval[3] = new ParameterValue();
                    paramval[3].Name = "T_End";
                    paramval[3].Value = ReportingEndDate.ToShortDateString();
                    paramval[4] = new ParameterValue();
                    paramval[4].Name = "Strategy_Status";
                    paramval[4].Value = strategy;
                    paramval[5] = new ParameterValue();
                    paramval[5].Name = "Tag";
                    paramval[5].Value = "Ungroup";
                    paramval[6] = new ParameterValue();
                    paramval[6].Name = "GroupByTag";
                    paramval[6].Value = "0";
                    break;
            }

            Warning[] warnings = null;
            string[] streamIDs = null;
            string encoding = string.Empty;
            string mimeType = string.Empty;
            string extension = string.Empty;
            var reportHtml = string.Empty;

            try
            {
                var rs = new ReportExecutionService();
                var reportUsername = ConfigurationManager.AppSettings["ReportUsername"];
                var reportPassword = ConfigurationManager.AppSettings["ReportPassword"];
                var reportDomain = ConfigurationManager.AppSettings["ReportDomain"];
                rs.Credentials = new NetworkCredential(reportUsername, reportPassword, reportDomain);
                rs.LoadReport(string.Format("{0}{1}", reportPathPrefix, reportName), null);
                rs.SetExecutionParameters(paramval, "en-us");
                reportHtml = System.Text.Encoding.Default.GetString(rs.Render("HTML4.0", string.Empty, out extension, out encoding, out mimeType, out warnings, out streamIDs));
                rs.Timeout = 10000;
            }
            catch (Exception ex)
            {
                _certonaService.Abort();
                throw;
            }
            
            return reportHtml;
        }

        /*
        public static ReportsViewModel GetRecommendations(string applicationId)
        {
            var model = new ReportsViewModel();
            model.Recommendations = new List<string>();

            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;

                        var getRequest = new GetApplicationSchemesRequest() { ApplicationID = applicationId, User = user };
                        var getResponse = _certonaService.GetApplicationSchemes(getRequest);
                        if (getResponse.Success && getResponse.Schemes != null && getResponse.Schemes.Count > 0)
                        {
                            foreach (var scheme in getResponse.Schemes)
                            {
                                model.Recommendations.Add(scheme.SchemeName);
                            }
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
                _certonaService.Abort();
                throw;
            }

            return model;
        }
        */

        public static ReportsViewModel GetUserAccountApplicationFeatures()
        {
            var model = new ReportsViewModel();
            model.ApplicationList = new List<string>();

            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var getRequest = new GetUserAccountApplicationFeaturesRequest() { User = user };
                        var getResponse = _certonaService.GetUserAccountApplicationFeatures(getRequest);
                        if (getResponse.Success && getResponse.Accounts[0].Applications != null && getResponse.Accounts[0].Applications.Count > 0)
                        {
                            model.ApplicationList =
                                getResponse.Accounts.Where(m => m.AccountID == user.LastAccountID)
                                           .SelectMany(m => m.Applications)
                                           .Select(m => m.ApplicationID)
                                           .ToList();
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
                _certonaService.Abort();
                throw;
            }

            return model;
        }

        public static ReportsViewModel GetSegments(string applicationId)
        {
            var model = new ReportsViewModel();
            model.Segments = new List<string>();
            
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        

                        var getRequest = new GetApplicationSegmentsRequest() { ApplicationID = applicationId, User = user };
                        var getResponse = _certonaService.GetApplicationSegments(getRequest);
                        if (getResponse.Success && getResponse.Segments != null && getResponse.Segments.Count > 0)
                        {
                            foreach (var name in getResponse.Segments)
                            {
                                model.Segments.Add(name);
                            }
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
                _certonaService.Abort();
                throw;
            }

            return model;
        }

        public static ReportsViewModel GetStrategies(string applicationId)
        {
            var model = new ReportsViewModel();
            model.Strategies = new List<string>();
            var strategies = "";
            
            var stratList = strategies.Split(',');
            foreach (var name in stratList)
            {
                model.Strategies.Add(name);
            }
            return model;
        }
        
        #endregion

        #region Trends 
        
        private static TrendsViewModel GetTrendsSeriesApplicationData(string id, DateTime startDate, DateTime endDate)
        {
            var model = new TrendsViewModel();
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;

                        model.TrendSeries = new List<TrendSeries>();

                        var getRequest = new GetTrendsDataApplicationRequest()
                                             {ApplicationID = id, StartDate = startDate, EndDate = endDate, User = user};
                        var getResponse = _certonaService.GetTrendsDataApplication(getRequest);
                        if (getResponse.Success && getResponse.TrendsData != null && getResponse.TrendsData.Count > 0)
                        {
                            foreach (var dto in getResponse.TrendsData)
                            {
                                var seriesDataPoint = new TrendSeries()
                                                          {Category = dto.Category, Group = dto.Group, Data = dto.Data};
                                model.TrendSeries.Add(seriesDataPoint);
                            }
                        }
                        else
                        {
                            var seriesDataPoint = new TrendSeries() {Category = "No Data", Group = "No Data"};
                            model.TrendSeries.Add(seriesDataPoint);
                        }

                        #region Dummy Data

                        //var series = new TrendSeries();
                        //series.Data = 2.8654690555282922; series.Group = "Shopping Cart Page"; series.Category = "Mar 2013";
                        //series.Data = 3.889; series.Group = "Shopping Cart Page"; series.Category = "Mar 2013";
                        //trendSeriesList.Add(series);
                        //series = new TrendSeries();
                        //series.Data = 3.936; series.Group = "Shopping Cart Page"; series.Category = "Apr 2013";
                        //trendSeriesList.Add(series);
                        //series = new TrendSeries();
                        //series.Data = 4.165; series.Group = "Shopping Cart Page"; series.Category = "March 2012";
                        //model.TrendSeries.Add(series);
                        //series = new TrendSeries();
                        //series.Data = 4.165; series.Group = "Shopping Cart Page"; series.Category = "Apr 2012";
                        //model.TrendSeries.Add(series);
                        //series = new TrendSeries();
                        //series.Data = 5.021; series.Group = "Shopping Cart Page"; series.Category = "May 2012";
                        //model.TrendSeries.Add(series);
                        //series = new TrendSeries();
                        //series.Data = 4.596; series.Group = "Shopping Cart Page"; series.Category = "Jun 2012";
                        //model.TrendSeries.Add(series);
                        //series = new TrendSeries();
                        //series.Data = 5.575; series.Group = "Shopping Cart Page"; series.Category = "Jul 2012";
                        //model.TrendSeries.Add(series);

                        //series = new TrendSeries();
                        //series.Data = .273; series.Group = "Home Page"; series.Category = "Jan 2012";
                        //model.TrendSeries.Add(series);
                        //series = new TrendSeries();
                        //series.Data = .325; series.Group = "Home Page"; series.Category = "Feb 2012";
                        //model.TrendSeries.Add(series);
                        //series = new TrendSeries();
                        //series.Data = .390; series.Group = "Home Page"; series.Category = "March 2012";
                        //model.TrendSeries.Add(series);
                        //series = new TrendSeries();
                        //series.Data = .458; series.Group = "Home Page"; series.Category = "Apr 2012";
                        //model.TrendSeries.Add(series);
                        //series = new TrendSeries();
                        //series.Data = .344; series.Group = "Home Page"; series.Category = "May 2012";
                        //model.TrendSeries.Add(series);
                        //series = new TrendSeries();
                        //series.Data = .225; series.Group = "Home Page"; series.Category = "Jun 2012";
                        //model.TrendSeries.Add(series);
                        //series = new TrendSeries();
                        //series.Data = .256; series.Group = "Home Page"; series.Category = "Jul 2012";
                        //model.TrendSeries.Add(series);

                        #endregion

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
            catch (Exception)
            {
                _certonaService.Abort();
                throw;
            }

            return model;
        }

        private static TrendsViewModel GetTrendsSeriesPackageData(int id, DateTime startDate, DateTime endDate)
        {
            var model = new TrendsViewModel();
            
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;

                        model.TrendSeries = new List<TrendSeries>();

                        var getRequest = new GetTrendsDataPackageRequest() { PackageID = id, StartDate = startDate, EndDate = endDate, User = user };
                        var getResponse = _certonaService.GetTrendsDataPackage(getRequest);
                        if (getResponse.Success && getResponse.TrendsData != null && getResponse.TrendsData.Count > 0)
                        {
                            foreach (var dto in getResponse.TrendsData)
                            {
                                var seriesDataPoint = new TrendSeries() { Category = dto.Category, Group = dto.Group, Data = dto.Data };
                                model.TrendSeries.Add(seriesDataPoint);
                            }
                        }
                        else
                        {
                            var seriesDataPoint = new TrendSeries() { Category = "No Data", Group = "No Data" };
                            model.TrendSeries.Add(seriesDataPoint);
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
                _certonaService.Abort();
                throw;
            }
            
            return model;
        }

        private static TrendsViewModel GetTrendsSeriesSchemeData(int id, DateTime startDate, DateTime endDate)
        {
            var model = new TrendsViewModel();

            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;

                        model.TrendSeries = new List<TrendSeries>();

                        var getRequest = new GetTrendsDataSchemeRequest() { SchemeID = id, StartDate = startDate, EndDate = endDate, User = user };
                        var getResponse = _certonaService.GetTrendsDataScheme(getRequest);
                        if (getResponse.Success && getResponse.TrendsData != null && getResponse.TrendsData.Count > 0)
                        {
                            foreach (var dto in getResponse.TrendsData)
                            {
                                var seriesDataPoint = new TrendSeries() { Category = dto.Category, Group = dto.Group, Data = dto.Data };
                                model.TrendSeries.Add(seriesDataPoint);
                            }
                        }
                        else
                        {
                            var seriesDataPoint = new TrendSeries() { Category = "No Data", Group = "No Data" };
                            model.TrendSeries.Add(seriesDataPoint);
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
                _certonaService.Abort();
                throw;

            }

            return model;
        }

        private static TrendsViewModel GetTrendsSeriesExperienceData(int id, DateTime startDate, DateTime endDate)
        {
            var model = new TrendsViewModel();

            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;

                        model.TrendSeries = new List<TrendSeries>();

                        var getRequest = new GetTrendsDataExperienceRequest() { ExperienceID = id, StartDate = startDate, EndDate = endDate, User = user };
                        var getResponse = _certonaService.GetTrendsDataExperience(getRequest);
                        if (getResponse.Success && getResponse.TrendsData != null && getResponse.TrendsData.Count > 0)
                        {
                            foreach (var dto in getResponse.TrendsData)
                            {
                                var seriesDataPoint = new TrendSeries() { Category = dto.Category, Group = dto.Group, Data = dto.Data };
                                model.TrendSeries.Add(seriesDataPoint);
                            }
                        }
                        else
                        {
                            var seriesDataPoint = new TrendSeries() { Category = "No Data", Group = "No Data" };
                            model.TrendSeries.Add(seriesDataPoint);
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
                _certonaService.Abort();
                throw;
            }

            return model;
        }

        private static TrendsViewModel GetTrendsSeriesVariantData(int id, DateTime startDate, DateTime endDate)
        {
            var model = new TrendsViewModel();
            
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;

                        model.TrendSeries = new List<TrendSeries>();

                        var getRequest = new GetTrendsDataVariantRequest() { VariantID = id, StartDate = startDate, EndDate = endDate, User = user };
                        var getResponse = _certonaService.GetTrendsDataVariant(getRequest);
                        if (getResponse.Success && getResponse.TrendsData != null && getResponse.TrendsData.Count > 0)
                        {
                            foreach (var dto in getResponse.TrendsData)
                            {
                                var seriesDataPoint = new TrendSeries() { Category = dto.Category, Group = dto.Group, Data = dto.Data };
                                model.TrendSeries.Add(seriesDataPoint);
                            }
                        }
                        else
                        {
                            var seriesDataPoint = new TrendSeries() { Category = "No Data", Group = "No Data" };
                            model.TrendSeries.Add(seriesDataPoint);
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
                _certonaService.Abort();
                throw;
            }

            return model;
        }
        
        private static TrendsViewModel CreateTrendsViewModel(string id, string title, string chartName, NodeType nodeType)
        {
            var model = new TrendsViewModel()
                            {
                                Title = title,
                                ChartName = chartName,
                                NodeType = nodeType,
                                StartDate = ReportingStartDate,
                                EndDate = ReportingEndDate,
                                ID = id,
                                TrendSeries = new List<TrendSeries>()
                            };

            model.ReportingLastUpdated = ReportingDateLastUpdated();
           return model;
        }

        private static DateTime? ReportingDateLastUpdated()
        {
            DateTime? reportingDataLastUpdated = null;
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;

                        var getRequest = new GetReportingDataLastUpdatedRequest() { User = user };
                        var getResponse = _certonaService.GetReportingDataLastUpdated(getRequest);
                        if (getResponse.Success && getResponse.ReportingDataLastUpdated != null)
                        {
                            reportingDataLastUpdated = getResponse.ReportingDataLastUpdated.ReportingDataLastUpdated;
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
                _certonaService.Abort();
                throw;
            }

            return reportingDataLastUpdated;
        }

        public static TrendsViewModel GetTrendsApplicationData(string id)
        {
            return CreateTrendsViewModel(id, REVENUE_CONTRIBUTION, "chart_trends_application", NodeType.Application);
        }

        public static TrendsViewModel GetTrendsPackageData(int id)
        {
            return CreateTrendsViewModel(id.ToString(), REVENUE_CONTRIBUTION, "chart_trends_package", NodeType.Package);
        }

        public static TrendsViewModel GetTrendsSchemeData(int id)
        {
            return CreateTrendsViewModel(id.ToString(), REVENUE_CONTRIBUTION, "chart_trends_scheme", NodeType.Scheme);
        }

        public static TrendsViewModel GetTrendsExperienceData(int id)
        {
            return CreateTrendsViewModel(id.ToString(), REVENUE_CONTRIBUTION, "chart_trends_experience", NodeType.Experience);
        }

        public static TrendsViewModel GetTrendsVariantData(int id)
        {
            return CreateTrendsViewModel(id.ToString(), REVENUE_CONTRIBUTION, "chart_trends_variant", NodeType.Variant);
        }

        public static TrendsViewModel RefreshTrends(int startYear, int startMonth, int startDay, int endYear, int endMonth, int endDay, string ID, NodeType nodeType)
        {
            // only update the series data
            var startDate = new DateTime(startYear, startMonth, startDay);
            var endDate = new DateTime(endYear, endMonth, endDay);

            ReportingStartDate = startDate;
            ReportingEndDate = endDate;

            TrendsViewModel model = null;
            switch (nodeType)
            {
                case NodeType.Application:
                    model = GetTrendsSeriesApplicationData(ID, startDate, endDate);
                    break;
                case NodeType.Package:
                    model = GetTrendsSeriesPackageData(int.Parse(ID), startDate, endDate);
                    break;
                case NodeType.Scheme:
                    model = GetTrendsSeriesSchemeData(int.Parse(ID), startDate, endDate);
                    break;
                case NodeType.Experience:
                    model = GetTrendsSeriesExperienceData(int.Parse(ID), startDate, endDate);
                    break;
                case NodeType.Variant:
                    model = GetTrendsSeriesVariantData(int.Parse(ID), startDate, endDate);
                    break;
                default:
                    throw new ApplicationException(String.Format("RefreshTrends(): Unsupported Node Type {0}", nodeType.ToString()));
            }
            
            #region Dummy Data
            //string group = "Dynamic Page";                        
            //var series = new TrendSeries();
            //series.Data = 100.273; series.Group = group; series.Category = "Jan 2012";
            //model.TrendSeries.Add(series);
            //series = new TrendSeries();
            //series.Data = 99.325; series.Group = group; series.Category = "Feb 2012";
            //model.TrendSeries.Add(series);
            //series = new TrendSeries();
            ////series.Data = 77.390; series.Group = group; series.Category = "March 2012";
            //series.Data = null; series.Group = group; series.Category = "March 2012";
            //model.TrendSeries.Add(series);
            //series = new TrendSeries();
            //series.Data = 88.458; series.Group = group; series.Category = "Apr 2012";
            //model.TrendSeries.Add(series);
            //series = new TrendSeries();
            //series.Data = 99.344; series.Group = group; series.Category = "May 2012";
            //model.TrendSeries.Add(series);
            //series = new TrendSeries();
            //series.Data = 92.225; series.Group = group; series.Category = "Jun 2012";
            //model.TrendSeries.Add(series);
            //series = new TrendSeries();
            //series.Data = 85.256; series.Group = group; series.Category = "Jul 2012";
            //model.TrendSeries.Add(series);
            #endregion

            return model;
        }

        #endregion

    }
}