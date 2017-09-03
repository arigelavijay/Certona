using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Resonance.Insight3.Web.ViewModels.Asset
{
    public class AssetDetails
    {
        public Guid? ID { get; set; }
        public string AccountItemID { get; set; }
        public string ImageURL { get; set; }
        public string Name { get; set; }
        public string DetailURL { get; set; }
        public DateTime? DateIntroduced { get; set; }
        public DateTime? DateLastUpdated { get; set; }
        public string Status { get; set; }
        public bool? ExcludedFromRecommendations { get; set; }

        //
        public string CatalogName { get; set; }
        public string CatalogID { get; set; }
        public string CatalogImage { get; set; }
    }
}