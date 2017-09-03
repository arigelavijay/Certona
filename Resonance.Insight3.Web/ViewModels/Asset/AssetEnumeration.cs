using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Resonance.Insight3.Web.ViewModels.Asset
{
    [Serializable]
    public class AssetEnumeration
    {
        public List<string> Values { get; set; }
        public string CatalogID { get; set; }
        public string Name { get; set; }
    }
}