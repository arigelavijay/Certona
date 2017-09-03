using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Resonance.Insight3.Web.ViewModels.Asset
{
    public class AssetDetailsViewModel
    {
        public AssetDetailsViewModel()
        {
            AdditionalDetails = new AssetAdditionalDetails
                                    {
                                        Details = new Dictionary<string, CatalogAssetMapping>()
                                    };
        }

        public AssetDetails Details { get; set; }
        public AssetAdditionalDetails AdditionalDetails { get; set; }
    }
}