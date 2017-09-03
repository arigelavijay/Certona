using System;
using System.Web.Mvc;
using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.Models;

namespace Resonance.Insight3.Web.Controllers
{
    public class ReportingController : Controller
    {
        public PartialViewResult ViewReport(NodeDTO node)
        {
            string cultureName = null;
            if (HttpContext != null && HttpContext.Request != null && HttpContext.Request.UserLanguages != null && HttpContext.Request.UserLanguages.Length > 0)
                cultureName = HttpContext.Request.UserLanguages[0];
        
            var model = ReportingModel.GetReportViewModel();
            return PartialView(model);
        }

        [HttpGet]
        public string GetReportData(int startYear,
            int startMonth,
            int startDay,
            int endYear,
            int endMonth,
            int endDay,
            string nodeId,
            string appId,
            //string segmentId,
            string schemeId,
            string strategy)
        {
            var startDate = new DateTime(startYear, startMonth, startDay);
            var endDate = new DateTime(endYear, endMonth, endDay);

            string cultureName = null;
            if (HttpContext != null && HttpContext.Request != null && HttpContext.Request.UserLanguages != null && HttpContext.Request.UserLanguages.Length > 0)
                cultureName = HttpContext.Request.UserLanguages[0];

            //var htmlReport = Convert.ToString(ReportingModel.GetReportDataModel(startDate, endDate, nodeId, appId, schemeId, cultureName, strategy));
            var htmlReport = Convert.ToString(ReportingModel.GetReportDataModel(startDate, endDate, nodeId, appId, cultureName, strategy));
            return htmlReport;
        }

        /*
        [HttpGet]
        public JsonResult GetRecommendations(string applicationId)
        {
            var model = ReportingModel.GetRecommendations(applicationId);
            return Json(model.Recommendations, JsonRequestBehavior.AllowGet);
        }
        */ 

        [HttpGet]
        public JsonResult GetUserAccountApplicationFeatures()
        {
            var model = ReportingModel.GetUserAccountApplicationFeatures();
            return Json(model.ApplicationList, JsonRequestBehavior.AllowGet);
        }

        /*
        [HttpGet]
        public JsonResult GetSegments(string applicationId)
        {
            var model = ReportingModel.GetSegments(applicationId);
            return Json(model.Segments, JsonRequestBehavior.AllowGet);
        }
        */ 

        [HttpGet]
        public JsonResult GetStrategies(string applicationId)
        {
            var model = ReportingModel.GetStrategies(applicationId);
            return Json(model.Strategies, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ViewTrends()
        {
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetTrendsData(int startYear, int startMonth, int startDay, int endYear, int endMonth, int endDay, string ID, NodeType nodeType)
        {
            var model = ReportingModel.RefreshTrends(startYear, startMonth, startDay, endYear, endMonth, endDay, ID, nodeType);
            return Json(model.TrendSeries, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTrendsModel(int startYear, int startMonth, int startDay, int endYear, int endMonth, int endDay, string ID, NodeType nodeType)
        {
            var model = ReportingModel.RefreshTrends(startYear, startMonth, startDay, endYear, endMonth, endDay, ID, nodeType);
            return Json(model.TrendSeries);
        }
    }
}
