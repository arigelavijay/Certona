using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Resonance.Insight3.Web.ViewModels.Asset
{
    public class CatalogAssetMapping
    {
        public string AttributeType { get; set; }
        public string DisplayLabel { get; set; }
        public List<string> DisplayValues { get; set; }
    }
}