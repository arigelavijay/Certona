using System;
using System.Collections.Generic;
using Resonance.Insight3.Web.CertonaService;

namespace Resonance.Insight3.Web.ViewModels.Reporting
{
    public class TrendsViewModel
    {
        private DateTime _endDate;

        public string ChartName { get; set; }
        public string Title { get; set; }        
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

        public string ID { get; set; }
        public List<TrendSeries> TrendSeries { get; set; }
        public NodeType NodeType { get; set; }
        public DateTime? ReportingLastUpdated { get; set; }
        public IEnumerable<string> SeriesColors
        {
            get
            {
                var seriesColors = new List<string>();
                seriesColors.Add("#0095EE");
                seriesColors.Add("#0012F1");
                seriesColors.Add("#FFBB00");
                seriesColors.Add("#00F177");
                seriesColors.Add("#FF8C00");
                seriesColors.Add("#FF3D00");
                seriesColors.Add("#FF6BC8");
                seriesColors.Add("#21F000");
                seriesColors.Add("#6FC4F7");
                seriesColors.Add("#D281B3");
                seriesColors.Add("#2D37B4");
                seriesColors.Add("#7079F8");
                seriesColors.Add("#BF9930");
                seriesColors.Add("#FFDA73");
                seriesColors.Add("#2DB570");
                seriesColors.Add("#70F8B3");
                seriesColors.Add("#A65B00");
                seriesColors.Add("#FFA940");
                seriesColors.Add("#A62800");
                seriesColors.Add("#FF9473");
                seriesColors.Add("#FFAEE1");
                seriesColors.Add("#A62375");
                seriesColors.Add("#82F86F");
                seriesColors.Add("#3FB42D");
                return seriesColors;
            }
        }
    }

    public class TrendSeries
    {
        public string Group { get; set; }
        public string Category { get; set; }
        public double? Data { get; set; }
    }
}