using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Resonance.Insight3.Web.ViewModels.Scheme
{
    public class SchemeContextExclusion
    {
        public bool? ExcludeCurrentAssets { get; set; }
        public bool? ExcludePreviousAssets { get; set; }
        public int ExcludeMaxCount { get; set; }
        public int ExcludeLastPages { get; set; }
        public int ExcludeDaysPast { get; set; }
        // Behavior Event Classes
        public List<SchemeEventContext> EventContexts = new List<SchemeEventContext>();
    }
}