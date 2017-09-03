using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Resonance.Insight3.Web.ViewModels.Reporting
{
    public class ReportsViewModel
    {
        private DateTime _endDate;

        public string ServerEndpoint { get; set; }
        public string Account { get; set; }
        public string Application { get; set; }
        public string ReportName { get; set; }
        public string ReportDisplayName { get; set; }
        public string AppID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate
        {
            set { _endDate = value; }

            get
            {
                if (_endDate > DateTime.Today.AddDays(-1))
                {
                    _endDate = DateTime.Today.AddDays(-1);
                }
                return _endDate;
            }
        }
        public string ReportUri { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string ReportHtml { get; set; }
        public List<string> Recommendations { get; set; }
        public List<string> Segments { get; set; }
        public List<string> Strategies { get; set; } 
        public List<string> ApplicationList { get; set; }
    }
}