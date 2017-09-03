using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Resonance.Insight3.Web.ViewModels.Catalog
{
    public class CatalogMapping
    {
        public string CatalogID { get; set; }
        public string ResonanceField { get; set; }
        public string CustomerField { get; set; }
        public string Description { get; set; }
        public string AttributeType { get; set; }
        public string CustomVariable { get; set; }
        public bool IsRuleEnabled { get; set; }
        public bool IsMapped { get; set; }
        public string XSLTransform { get; set; }
        public string OLAPLevel { get; set; }
    }
}