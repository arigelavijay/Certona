using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Resonance.Insight3.Web.ViewModels.Reporting
{
    public class ReportsViewModel
    {
        public string ServerEndpoint { get; set; }
        public string Account { get; set; }
        public string Application { get; set; }
        public string ReportName { get; set; }
        public string ReportDisplayName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ReportUri { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }

        public List<SelectListItem> ApplicationList { get; set; }
    }
}
