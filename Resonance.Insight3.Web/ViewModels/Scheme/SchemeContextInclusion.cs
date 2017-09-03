using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Resonance.Insight3.Web.ViewModels.Scheme
{
    public class SchemeContextInclusion
    {
        public bool? IncludeCurrentAssets { get; set; }
        public bool? IncludePreviousAssets { get; set; }
        public int IncludeMaxCount { get; set; }
        public int IncludeLastPages { get; set; }
        public int IncludeDaysPast { get; set; }
        // Behavior Event Classes
        public List<SchemeEventContext> EventContexts = new List<SchemeEventContext>();
    }
}