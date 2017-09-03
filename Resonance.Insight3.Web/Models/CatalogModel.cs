using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel;
using System.Text;
using System.Web;
using Kendo.Mvc;
using Kendo.Mvc.Infrastructure.Implementation;
using Kendo.Mvc.UI;
using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.FormAuthentication;
using Resonance.Insight3.Web.ViewModels.Catalog;
using System.Linq;
using System.Reflection;

namespace Resonance.Insight3.Web.Models
{
    public class CatalogModel : ModelBase
    {
        public static HeaderViewModel GetCatalogHeader(string catalogID)
        {
            var vm = new HeaderViewModel();

            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var catalogDetailsRequest = new GetCatalogDetailsRequest() { User = user, CatalogID = catalogID };
                        var catalogDetailsResponse = _certonaService.GetCatalogDetails(catalogDetailsRequest);


                        if (catalogDetailsResponse.Success && catalogDetailsResponse.CatalogDetails != null)
                        {
                            vm.CatalogName = catalogDetailsResponse.CatalogDetails.Name;
                        }
                    }
                    catch (TimeoutException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                    catch (CommunicationException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return vm;
        }

        public static CatalogDetails GetCatalogDetails(string catalogID)
        {
            CatalogDetails details = null;
            try
            {
                using(_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var catalogDetailsRequest = new GetCatalogDetailsRequest() { User = user, CatalogID = catalogID };
                        var catalogDetailsResponse = _certonaService.GetCatalogDetails(catalogDetailsRequest);


                        if (catalogDetailsResponse.Success && catalogDetailsResponse.CatalogDetails != null)
                        {
                            details = new CatalogDetails()
                            {
                                CatalogId = catalogID,
                                Name = catalogDetailsResponse.CatalogDetails.Name,
                                Description = catalogDetailsResponse.CatalogDetails.Description,
                                AssetType = catalogDetailsResponse.CatalogDetails.AssetType,
                                Icon_FileName = catalogDetailsResponse.CatalogDetails.Icon_FileName,
                                CatalogApplications = catalogDetailsResponse.CatalogDetails.Applications,
                                LanguageName = catalogDetailsResponse.CatalogDetails.LanguageName
                            };
                        }
                    }
                    catch (TimeoutException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                    catch (CommunicationException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return details;
        }

        public static CatalogItemDetails GetCatalogItem(string catalogID, string accountItemID)
        {
            CatalogItemDetails details = null;
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var catalogItemRequest = new GetCatalogItemRequest()
                                                     {
                                                         User = user,
                                                         AccountID = user.LastAccountID,
                                                         AccountItemID = accountItemID,
                                                         CatalogID = catalogID
                                                     };

                        var catalogItemResponse = _certonaService.GetCatalogItem(catalogItemRequest);

                        if (catalogItemResponse.Success && catalogItemResponse.CatalogItem != null)
                        {
                            details = new CatalogItemDetails()
                                {
                                    CatalogAssociatedItems = catalogItemResponse.CatalogItem.CatalogItemAssociatedItem.Select(
                                    r => new ItemAssociatedItem
                                        {
                                            ItemID = r.ItemID,
                                            DisplayOrder = r.DisplayOrder
                                        }).ToList()
                                };

                            if (catalogItemResponse.CatalogItem.CatalogItemInfo != null)
                                details.CatalogItemInfo = new ItemInfo(catalogItemResponse.CatalogItem.CatalogItemInfo);

                            details.CatalogItemMetaData = catalogItemResponse.CatalogItem.CatalogItemMetaData.Select(
                                r => new ItemMetaData
                                    {
                                        CatalogID = r.CatalogID,
                                        FunctionName = r.FunctionName,
                                        LevelName = r.LevelName,
                                        GroupName = r.GroupName
                                    }).ToList();

                            details.CatalogMetaData = catalogItemResponse.CatalogItem.CatalogMetaData.Select(
                                r => new MetaData
                                    {
                                        CatalogID = r.CatalogID,
                                        Name1 = r.Name1,
                                        Name2 = r.Name2,
                                        Name3 = r.Name3
                                    }).ToList();

                            details.CatalogMetaData2 = catalogItemResponse.CatalogItem.CatalogMetaData2.Select(
                                r => new MetaData
                                {
                                    CatalogID = r.CatalogID,
                                    Name1 = r.Name1,
                                    Name2 = r.Name2,
                                    Name3 = r.Name3
                                }).ToList();

                            details.CatalogMapping = catalogItemResponse.CatalogItem.CatalogMapping.Select(
                                r => new CatalogMapping
                                    {
                                        CatalogID = r.CatalogID,
                                        ResonanceField =r.ResonanceField,
                                        CustomerField = r.CustomerField,
                                        Description = r.Description,
                                        AttributeType = r.AttributeType,
                                        CustomVariable = r.CustomVariable,
                                        IsRuleEnabled = r.IsRuleEnabled,
                                        IsMapped = r.IsMapped,
                                        XSLTransform = r.XSLTransform,
                                        OLAPLevel = r.OLAPLevel
                                    }).ToList();
                        }
                    }
                    catch (TimeoutException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                    catch (CommunicationException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return details;
        }

        public static AssetsViewModel GetCatalogMappingsForAccountItemIDAndDescription(string catalogId)
        {
            var ret = new AssetsViewModel();
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var idRequest = new GetCatalogMappingFromResonanceFieldRequest
                            {
                                CatalogID = catalogId,
                                User = user,
                                ResonanceField = "AccountItemID"
                            };
                        var idResponse = _certonaService.GetCatalogMappingFromResonanceField(idRequest);
                        ret.AccountItemIDMapping = idResponse.Success ? !String.IsNullOrWhiteSpace(idResponse.CatalogMapping) ? idResponse.CatalogMapping : "AccountItemID" : "AccountItemID";

                        var descriptionRequest = new GetCatalogMappingFromResonanceFieldRequest
                        {
                            CatalogID = catalogId,
                            User = user,
                            ResonanceField = "Description"
                        };
                        var descriptionResponse = _certonaService.GetCatalogMappingFromResonanceField(descriptionRequest);
                        ret.DescriptionMapping = idResponse.Success ? !String.IsNullOrWhiteSpace(descriptionResponse.CatalogMapping) ? descriptionResponse.CatalogMapping : "AccountItemID" : "AccountItemID";
                    }
                    catch (TimeoutException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                    catch (CommunicationException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return ret;
        }

        public static void BindDataSourceRequest(HttpRequestBase httpRequest, ref DataSourceRequest dataSourceRequest)
        {
            //Bind Sorts
            dataSourceRequest.Sorts = new List<SortDescriptor>();
            foreach (var field in httpRequest.Params.AllKeys.Where(k => k.StartsWith("sort") && k.EndsWith("[field]")))
            {
                var fldVal = httpRequest.Params.Get(field);
                var dirVal = httpRequest.Params.Get(field.Replace("[field]", "[dir]"));
                dataSourceRequest.Sorts.Add(new SortDescriptor(fldVal, dirVal.ToUpper() == "ASC" ? ListSortDirection.Ascending : ListSortDirection.Descending));
            }

            //Bind Filters
            dataSourceRequest.Filters = new List<IFilterDescriptor>();
            foreach (var field in httpRequest.Params.AllKeys.Where(k => k.StartsWith("filter") && k.EndsWith("[field]")))
            {
                var fldVal = httpRequest.Params.Get(field);
                var operatorVal = httpRequest.Params.Get(field.Replace("[field]", "[operator]"));
                var fltOp = FilterOperator.Contains;
                switch(operatorVal)
                {
                    case "eq":
                    case "==":
                    case "isequalto":
                    case "equals":
                    case "equalto":
                    case "equal":
                        fltOp = FilterOperator.IsEqualTo;
                        break;
                    case "neq":
                    case "!=":
                    case "isnotequalto":
                    case "notequals":
                    case "notequalto":
                    case "notequal":
                    case "ne":
                        fltOp = FilterOperator.IsNotEqualTo;
                        break;
                    case "lt":
                    case "<":
                    case "islessthan":
                    case "lessthan":
                    case "less":
                        fltOp = FilterOperator.IsLessThan;
                        break;
                    case "lte":
                    case "<=":
                    case "islessthanorequalto":
                    case "lessthanequal":
                    case "le":
                        fltOp = FilterOperator.IsLessThanOrEqualTo;
                        break;
                    case "gt":
                    case ">":
                    case "isgreaterthan":
                    case "greaterthan":
                    case "greater":
                        fltOp = FilterOperator.IsGreaterThan;
                        break;
                    case "gte":
                    case ">=":
                    case "isgreaterthanorequalto":
                    case "greaterthanequal":
                    case "ge":
                        fltOp = FilterOperator.IsGreaterThanOrEqualTo;
                        break;
                    case "startswith":
                        fltOp = FilterOperator.StartsWith;
                        break;
                    case "endswith":
                        fltOp = FilterOperator.EndsWith;
                        break;
                    case "contains":
                    case "substringof":
                        fltOp = FilterOperator.Contains;
                        break;
                }
                var val = httpRequest.Params.Get(field.Replace("[field]", "[value]"));
                dataSourceRequest.Filters.Add(new FilterDescriptor(fldVal, fltOp, val));
            }
        }

        public static GetAssetsModelResponse GetCatalogItems(DataSourceRequest request, string catalogId)
        {
            var ret = new GetAssetsModelResponse();

            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var getItemsRequest = new GetCatalogItemsRequest
                        {
                            CatalogID = catalogId,
                            User = user
                        };

                        foreach (FilterDescriptor filter in request.Filters)
                        {
                            switch (filter.Member.ToLower())
                            {
                                case "description":
                                    getItemsRequest.NameValueFilter = filter.Value.ToString();
                                    getItemsRequest.NameValueOperator = "Contains";
                                    break;
                                case "account_item_id":
                                    getItemsRequest.IDValueFilter = filter.Value.ToString();
                                    getItemsRequest.IDValueOperator = "Starts With";
                                    break;
                                case "disabled":
                                    getItemsRequest.StatusValueFilter = filter.Value.ToString() == "1";
                                    break;
                            }
                        }

                        var sbSort = new StringBuilder();

                        foreach (var sort in request.Sorts)
                        {
                            var member = "";
                            switch (sort.Member.ToLower())
                            {
                                case "account_item_id":
                                    member = "Account_Item_ID";
                                    break;
                                case "description":
                                    member = "Description";
                                    break;
                                default:
                                    member = "Disabled";
                                    break;
                            }
                            sbSort.AppendFormat(
                                sort.SortDirection == ListSortDirection.Ascending ? "{0} asc, " : "{0} desc, ",
                                member);
                        }

                        var trimChars = new char[2];
                        trimChars[0] = ',';
                        trimChars[1] = ' ';

                        getItemsRequest.SortColumns = sbSort.ToString().TrimEnd(trimChars);
                        getItemsRequest.Page = request.Page;
                        getItemsRequest.PageSize = request.PageSize;

                        var response = _certonaService.GetCatalogItems(getItemsRequest);

                        if (response.Success)
                        {
                            ret.Items = response.Items;
                            ret.Total = response.Total;
                        }
                        else
                        {
                            ret.Total = 0;
                        }
                    }
                    catch (TimeoutException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                    catch (CommunicationException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return ret;
        }
    }
}