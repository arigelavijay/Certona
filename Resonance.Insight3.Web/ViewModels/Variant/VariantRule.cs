using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Resonance.Insight3.Web.ViewModels.Variant
{
    public class VariantRule
    {
        public int VariantID { get; set; }
        public int RuleID { get; set; }
        public string Name { get; set; }
        public string RuleText { get; set; }
    }
}