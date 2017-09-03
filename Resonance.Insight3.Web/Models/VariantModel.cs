using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Web.Mvc;
using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.FormAuthentication;
using Resonance.Insight3.Web.ViewModels.Variant;

namespace Resonance.Insight3.Web.Models
{
    public class VariantModel : ModelBase
    {
        public static List<RecommendationTypeDTO> GetRecTypeList(string catalogID)
        {
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var request = new GetRecommendationTypesRequest() { User = user, CatalogID = catalogID };
                        var response = _certonaService.GetRecommendationTypes(request);
                        return response.RecommendationTypes;
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
        }

        public static VariantDetails GetVariantDetails(int variantID)
        {
            VariantDetails details = null;
            int? nullMethodId = null;

            #region Main
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var variantDetailsRequest = new GetVariantDetailsRequest() { User = user, VariantID = variantID };
                        var variantDetailsResponse = _certonaService.GetVariantDetails(variantDetailsRequest);
                        
                        if (variantDetailsResponse.Success && variantDetailsResponse.VariantDetails != null)
                        {
                            details = new VariantDetails()
                            {
                                VariantId = variantID,
                                Catalog = variantDetailsResponse.VariantDetails.Catalog,
                                CatalogId = variantDetailsResponse.VariantDetails.CatalogID, 
                                ContainerTitle = variantDetailsResponse.VariantDetails.ContainerTitle,
                                Description = variantDetailsResponse.VariantDetails.Description,
                                Icon_FileName = variantDetailsResponse.VariantDetails.Icon_FileName,
                                Name = variantDetailsResponse.VariantDetails.Name,
                                NumberOfAssets = variantDetailsResponse.VariantDetails.NumberOfAssets,
                                PersonalizationType = variantDetailsResponse.VariantDetails.PersonalizationType,
                                MethodId = variantDetailsResponse.VariantDetails.MethodID,
                                PoolFactor = variantDetailsResponse.VariantDetails.PoolFactor,
                                Priority = variantDetailsResponse.VariantDetails.Priority,
                                RecommendationType = variantDetailsResponse.VariantDetails.RecommendationType,
                                SubjectWeight = variantDetailsResponse.VariantDetails.SubjectWeight,
                                Status = variantDetailsResponse.VariantDetails.Status
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

            }
            #endregion

            //if (details != null)
            //{
            //    if (!string.IsNullOrEmpty(details.CatalogId))
            //    {
            //        var recTypeList = VariantModel.GetRecTypeList(details.CatalogId);
            //        details.PersonalizationTypeList = recTypeList.Select(s => new SelectListItem
            //                    {
            //                        Text = s.Name,
            //                        Value = s.MethodID.ToString(),
            //                        Selected = (s.Name == details.PersonalizationType) ? true : false
            //                    }).ToList();
            //    }
            //
            //    var statusList = ApplicationModel.GetStatusList();
            //    details.StatusList = statusList.Select(s => new SelectListItem { Text = s, Value = s }).ToList();           
            //}

            return details;
        }

        public static List<VariantRule> GetVariantRulesForDetailGrid(int variantID)
        {
            List<VariantRule> list = GetVariantRules(variantID);
            foreach(var vrule in list)
            {
                // vrule.RuleText = "<i>This strategy will return results</i>&#160;&#160;" + vrule.RuleText;
                // replace the first <div> with content
                if (vrule.RuleText.StartsWith("<div>"))
                    vrule.RuleText = "<div>This strategy will return results" + vrule.RuleText.Substring(5, vrule.RuleText.Length - 5);                
                vrule.Name = "<span class='bold'>Rule Name:</span>&#160;" + vrule.Name;
            }
            return list;
        }

        public static List<VariantRule> GetVariantRules(int variantID)
        {
            var rules = new List<VariantRule>();
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var request = new GetVariantRulesRequest() { User = user, VariantID = variantID };
                        var response = _certonaService.GetVariantRules(request);

                        if (response.Success && response.Rules != null)
                        {
                            foreach (var rule in response.Rules)
                            {
                                rules.Add(new VariantRule()
                                {
                                    VariantID = variantID,
                                    RuleID = rule.RuleID,
                                    Name = rule.RuleName,
                                    RuleText = rule.ExpressionText
                                });
                            }

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

            }

            return rules;
        }

        public static bool UpdateVariantModifiers(string variantId, int pool, int profile, List<VariantBiases> biases, List<ViewModels.Variant.VariantModel> variants)
        {
            var isSuccesful = false;
            using (_certonaService = new CertonaServiceClient())
            {
                try
                {
                    var biasDTO = new List<VariantBiasDTO>();
                    var variantDTO = new List<VariantModelDTO>();

                    var details = new VariantDetailsDTO();
                    details.VariantId = variantId;
                    details.PoolFactor = pool;
                    details.SubjectWeight = profile;

                    if (biases != null)
                    {
                        biasDTO = biases.Select(m => new VariantBiasDTO()
                            {
                                BiasID = m.BiasID,
                                Weight = Convert.ToDouble("." + m.Weight)
                            }).ToList();
                    }

                    if (variants != null)
                    {
                        variantDTO = variants.Select(m => new VariantModelDTO()
                            {
                                ModelID = m.ModelID,
                                Weight = m.Weight
                            }).ToList();
                    }

                    UserDTO user = FormsAuthenticationWrapper.User;
                    var request = new UpdateVariantModifiersRequest
                    {
                        User = user,
                        VariantDetails = details,
                        VariantBiases = biasDTO,
                        VariantModels = variantDTO
                    };
                    var response = _certonaService.UpdateVariantModifiers(request);
                    
                    if (response.Success)
                    {
                         isSuccesful = true;
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

            return isSuccesful;
        }

        public static VariantBoosts GetVariantBoosts(int variantID)
        {
            VariantBoosts details = null;
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var getRequest = new GetVariantBoostsRequest() { User = user, VariantID = variantID };
                        var getResponse = _certonaService.GetVariantBoosts(getRequest);

                        if (getResponse.Success)
                        {
                            details = new VariantBoosts { VariantID = variantID };

                            details.VariantDetails = new VariantDetails()
                                                         {
                                                             PoolFactor = getResponse.VariantDetails.PoolFactor,
                                                             SubjectWeight = getResponse.VariantDetails.SubjectWeight
                                                         };

                            details.VariantBiases = new List<VariantBiases>();
                            details.VariantBiases.AddRange(getResponse.VariantBiases.Select(b => new VariantBiases()
                                                                                                     {
                                                                                                         BiasID = b.BiasID,
                                                                                                         ListName = b.ListName,
                                                                                                         Weight = b.Weight
                                                                                                     } ).ToList());


                            details.VariantModels = new List<ViewModels.Variant.VariantModel>();
                            details.VariantModels.AddRange(getResponse.VariantModels.Select(m => new ViewModels.Variant.VariantModel()
                                                                                                     {
                                                                                                        CatalogID = m.CatalogID,
                                                                                                        Description = m.Description,
                                                                                                        ModelID = m.ModelID,
                                                                                                        Name = m.Name,
                                                                                                        Weight = m.Weight
                                                                                                     }).ToList());                                
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

        public static List<int> GetVariantSlotList(int variantID)
        {
            var slots = new List<int>();
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var request = new GetVariantSlotsRequest() { User = user, VariantID = variantID };
                        var response = _certonaService.GetVariantSlots(request);

                        if (response.Success && response.Slots != null)
                        {
                            slots.AddRange(response.Slots);
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

            return slots;
        }

        public static string GetVariantSlotString(int variantID)
        {
            StringBuilder sb = new StringBuilder();
            
            List<int> slots = GetVariantSlotList(variantID);
            if (slots.Count == 0)
                return "All";

            //int counter = 0;
            //foreach(var slot in slots)
            //{
            //    sb.Append(string.Format("{0}{1}", slot, (counter < slots.Count ? "," : "")));
            //    counter++;
            //}

            // Need logic to format list: 1,2,3,4,5,8,10,11,12            
            // To string equivalent: 1,3-5,8,10-12

            int count = slots.Count - 1;
            int startSlot = 0;
            int endSlot = 0;

            for (int c = 0; c <= count; c++ )
            {
                var comma = (c == count ? string.Empty : ",");

                // get slot
                var slot = slots[c];
                var nextSlot = 0;

                if (startSlot == 0)
                    startSlot = slot;
                if (endSlot == 0)
                    endSlot = slot;

                // get next one if available
                if (c != count)
                    nextSlot = slots[c + 1];
                else
                    nextSlot = endSlot;

                // if next one is in sequential order, continue processing
                if (nextSlot == endSlot + 1)
                {
                    endSlot = nextSlot;
                    if (c != count)
                        continue;
                    else
                    {
                        // terminate cuz last one
                        if (startSlot == endSlot)
                        {
                            sb.Append(String.Format("{0}{1}", startSlot, comma));
                        }
                        else
                        {
                            sb.Append(String.Format("{0}-{1}{2}", startSlot, endSlot, comma));
                        }
                    }
                }
                else
                {
                    // empty space in sequence, so terminate
                    if (startSlot == endSlot)
                    {
                        sb.Append(String.Format("{0}{1}", startSlot, comma));
                    }
                    else
                    {
                        sb.Append(String.Format("{0}-{1}{2}", startSlot, endSlot, comma));    
                    }                    
                    // reset
                    startSlot = 0;
                    endSlot = 0;
                }                
            }

            return sb.ToString();
        }

        public static void UpdateVariantDetails(VariantDetails details)
        {
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var request = new UpdateVariantDetailsRequest()
                                          {
                                              User = user,
                                              VariantID = details.VariantId,
                                              Name = details.Name,
                                              Description = details.Description ?? "",
                                              Explanation = details.ContainerTitle ?? "",
                                              CatalogID = details.CatalogId,            // passed back as is
                                              MethodID = details.MethodId,
                                              Status = details.Status,
                                              NumberOfItems = details.NumberOfAssets, // passed back as is
                                              Priority = details.Priority // passed back as is
                                          };
                        var response = _certonaService.UpdateVariantDetails(request);
                        if (!response.Success)
                            throw new ApplicationException("UpdateVariantDetails returned failure status");
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
        }

        public static void UpdateVariantStatus(string variantID_List, string status)
        {
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var request = new UpdateVariantStatusRequest() { User = user, VariantID_List = variantID_List, Status = status};
                        var response = _certonaService.UpdateVariantStatus(request);
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
        }

        public static void UpdateVariantListPriority(string variantID_List, string action)
        {
            string[] idList = variantID_List.Split(',');
            if (action == "Move Up")
            {
                // Process in order
                foreach (string id in idList)
                {
                    UpdateVariantPriority(int.Parse(id), "up");
                }
            }
            else
            {
                // Process in reverse order
                for (int i = idList.Length - 1; i >= 0; i--)
                {
                    UpdateVariantPriority(int.Parse(idList[i]), "down");
                }
            }
        }

        private static void UpdateVariantPriority(int variantID, string action)
        {
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var request = new UpdateVariantPriorityRequest() { User = user, VariantID = variantID, Direction = action };
                        var response = _certonaService.UpdateVariantPriority(request);
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

        }

        public static int CreateVariant(VariantNodeViewModel model)
        {
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var request = new CreateVariantRequest()
                        {
                            User = user,
                            ExperienceID = model.ExperienceID,
                            Name = model.Name,
                            Description = model.Description,
                            Priority = model.Priority,
                            CatalogID = model.CatalogID,
                            ContainerName = model.ContainerTitle,
                            NumberOfAssets = model.NumberOfAssets,
                            MethodID = model.MethodID
                        };
                        var response = _certonaService.CreateVariant(request);
                        return response.VariantID;
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
        }
    }
}