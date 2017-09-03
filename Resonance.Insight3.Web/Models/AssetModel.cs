using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Resonance.Insight3.Web.ViewModels.Asset;
using Resonance.Insight3.Web.ViewModels.Catalog;

namespace Resonance.Insight3.Web.Models
{
    public static class AssetModel
    {
        public static AssetDetailsViewModel GetCatalogAssetDetails(string catalogID, string accountItemID)
        {
            CatalogItemDetails dataModel = CatalogModel.GetCatalogItem(catalogID, accountItemID);
            CatalogDetails catalog = CatalogModel.GetCatalogDetails(catalogID);

            var vm = new AssetDetailsViewModel
                         {
                             Details = MapAssetDetails(dataModel, catalog)
                         };

            if (string.IsNullOrEmpty(vm.Details.AccountItemID))
                vm.Details.AccountItemID = accountItemID;
            if (string.IsNullOrEmpty(vm.Details.CatalogID))
                vm.Details.CatalogID = catalogID;

            return vm;
        }

        private static AssetDetails MapAssetDetails(CatalogItemDetails dataModel, CatalogDetails catalog)
        {
            var returnObj = new AssetDetails();

            if (dataModel == null || dataModel.CatalogItemInfo == null)
                return returnObj;
            
            returnObj.ID = dataModel.CatalogItemInfo.ItemID;
            returnObj.AccountItemID = dataModel.CatalogItemInfo.AccountItemID;
            returnObj.ImageURL = dataModel.CatalogItemInfo.ImageUrl;
            returnObj.Name = dataModel.CatalogItemInfo.Description;
            returnObj.DetailURL = dataModel.CatalogItemInfo.DetailUrl;
            returnObj.DateIntroduced = dataModel.CatalogItemInfo.DateIntroduced;
            returnObj.DateLastUpdated = dataModel.CatalogItemInfo.LastUpdate;
            returnObj.Status = dataModel.CatalogItemInfo.Disabled ? "Inactive" : "Active";
            returnObj.ExcludedFromRecommendations = dataModel.CatalogItemInfo.ExcludedFromRecommendations;
            
            if (catalog != null)
            {
                returnObj.CatalogName = catalog.Name;
                returnObj.CatalogID = catalog.CatalogId;
                returnObj.CatalogImage = catalog.Icon_FileName;
            }

            return returnObj;
        }

        public static AssetDetailsViewModel GetCatalogAssetDetailsWithMapping(string catalogID, string accountItemID)
        {
            CatalogItemDetails dataModel = CatalogModel.GetCatalogItem(catalogID, accountItemID);
            CatalogDetails catalog = CatalogModel.GetCatalogDetails(catalogID);

            AssetDetails details = MapAssetDetails(dataModel,catalog);
            if (string.IsNullOrEmpty(details.AccountItemID))
                details.AccountItemID = accountItemID;
            if (string.IsNullOrEmpty(details.CatalogID))
                details.CatalogID = catalogID;

            AssetAdditionalDetails additionalDetails = new AssetAdditionalDetails();
            
            // Dictionary Keys come from dataModel.CatalogMapping
            foreach (var catalogMapping in dataModel.CatalogMapping.OrderBy(c => c.CustomerField))
            {
                additionalDetails.Details.Add(catalogMapping.ResonanceField, new CatalogAssetMapping()
                {
                    DisplayLabel = catalogMapping.CustomerField,
                    AttributeType = catalogMapping.AttributeType,
                    DisplayValues = new List<string>()
                });
            }

            // now we have map, need to iterate through CatalogItemMetadata
            foreach (var item in dataModel.CatalogItemMetaData)
            {
                // look up function name, then add to Display values as needed if 
                if (additionalDetails.Details.ContainsKey(item.FunctionName))
                {
                    // get object by key and update
                    CatalogAssetMapping objValue = null;
                    if (additionalDetails.Details.TryGetValue(item.FunctionName, out objValue))
                    {
                        objValue.DisplayValues.Add(item.GroupName);
                        objValue.DisplayValues.Sort();
                    }
                }
            }

            // map using Reflection
            if (dataModel.CatalogItemInfo != null)
            {
                PropertyInfo[] propertyInfos = dataModel.CatalogItemInfo.GetType().GetProperties();
                for (int i = 0; i < propertyInfos.Length; i++)
                {
                    var key = propertyInfos[i].Name;

                    // list of exceptions (these values have scheme names with _ but have been stripped out of the key)
                    switch (propertyInfos[i].Name)
                    {
                        case "ItemID":
                            key = "Item_ID";
                            break;
                        case "AccountID":
                            key = "Account_ID";
                            break;
                        case "AccountItemID":
                            key = "Account_Item_ID";
                            break;
                        case "ObjectID":
                            key = "Object_ID";
                            break;
                        case "CatalogID":
                            key = "Catalog_ID";
                            break;
                        case "ImageURL":
                            key = "Image_URL";
                            break;
                        case "DetailURL":
                            key = "Detail_URL";
                            break;
                    }

                    if (additionalDetails.Details.ContainsKey(key)) //|| additionalDetails.Details.ContainsKey(key2))
                    {
                        CatalogAssetMapping objValue = null;
                        // see if property is mapped, if so, then add the value
                        // if (additionalDetails.Details.TryGetValue(propertyInfos[i].Name, out objValue))
                        if (additionalDetails.Details.TryGetValue(key, out objValue))
                        {
                            // get the property value
                            var obj = propertyInfos[i].GetValue(dataModel.CatalogItemInfo, null);
                            if (obj != null)
                            {
                                objValue.DisplayValues.Add(obj.ToString());
                                objValue.DisplayValues.Sort();
                            }
                        }
                    }
                }
            }

            return new AssetDetailsViewModel
                       {
                           Details = details,
                           AdditionalDetails = additionalDetails
                       };
        }
    }

}