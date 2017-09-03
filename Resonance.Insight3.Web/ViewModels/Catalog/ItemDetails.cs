using System;
using System.Collections.Generic;

namespace Resonance.Insight3.Web.ViewModels.Catalog
{
    public class CatalogItemDetails
    {
        public List<ItemAssociatedItem> CatalogAssociatedItems { get; set; }
        public ItemInfo CatalogItemInfo { get; set; }
        public List<ItemMetaData> CatalogItemMetaData { get; set; }
        public List<MetaData> CatalogMetaData { get; set; }
        public List<MetaData> CatalogMetaData2 { get; set; }
        public List<CatalogMapping> CatalogMapping { get; set; }
    }
}