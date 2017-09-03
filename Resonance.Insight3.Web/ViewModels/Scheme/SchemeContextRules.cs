using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Resonance.Insight3.Web.ViewModels.Scheme
{
    public class SchemeContextRules
    {
        public int RulesMaxCount { get; set; }
        public int RulesLastPages { get; set; }
        public int RulesDaysPast { get; set; }
        // Behavior Event Classes
        public List<SchemeEventContext> EventContexts = new List<SchemeEventContext>();
    }
}