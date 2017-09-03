using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.FormAuthentication;
using Resonance.Insight3.Web.ViewModels.Experience;
using Resonance.Insight3.Web.ViewModels.Scheme;
using Resonance.Insight3.Web.ViewModels.Variant;

namespace Resonance.Insight3.Web.Models
{
    public class ExperienceModel : ModelBase
    {
        public static ExperienceNodeViewModel GetExperienceDetailsWithRules (int experienceID)
        {
            var model = ExperienceModel.GetExperienceDetails(experienceID);
            List<ExperienceStrategy> strategyList = ExperienceModel.GetExperienceStrategies(experienceID);
            model.StrategyCount = strategyList.Count;

            // Additional model client side binding
            List<VariantRule> rulesList = new List<VariantRule>();
            List<ExperienceStrategyDecisionPlan> decisionPlanList = new List<ExperienceStrategyDecisionPlan>();
            List<int> sList = new List<int>();

            foreach (var strategy in strategyList)
            {
                rulesList.AddRange(VariantModel.GetVariantRulesForDetailGrid(strategy.StrategyID));

                var str = VariantModel.GetVariantSlotString(strategy.StrategyID);

                decisionPlanList.Add(new ExperienceStrategyDecisionPlan()
                {
                    StrategyID = strategy.StrategyID,
                    NumberOfItems = strategy.NumberOfItems,
                    MinItems = strategy.MinItemsReturned,
                    BreakOnMinItems = strategy.BreakOnMinItemsReturned,
                    SlotInfo = str
                });

                sList.Add(strategy.StrategyID);
            }
            sList.Sort();
            model.DecisionPlans = decisionPlanList;
            model.VariantRules = rulesList;
            model.StrategyIDList = sList;

            return model;
        }

        public static ExperienceNodeViewModel CreateExperience()
        {
            var numberOfAssets = 0;
            var model = new ExperienceNodeViewModel();
            var startNumberOfPriorities = 1;
            var endNumberOfPriorities = 100;
            var startNumberOfAssets = 1;
            var endNumberOfAssets = 5;

            //model.Priority = Resonance.Insight3.Web.HtmlHelpers.FormHelpers.DropDownListInt(startNumberOfPriorities, endNumberOfPriorities + 1);
            //model.NumberOfAssets = Resonance.Insight3.Web.HtmlHelpers.FormHelpers.DropDownListInt(startNumberOfAssets, endNumberOfAssets + 1);

            return model;
        }

        public static ExperienceNodeViewModel GetExperienceDetails(int experienceID)
        {
            ExperienceNodeViewModel details = new ExperienceNodeViewModel();
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var request = new GetExperienceDetailsRequest() { User = user, ExperienceID = experienceID };
                        var response = _certonaService.GetExperienceDetails(request);
                        
                        if (response.Success && response.Details != null)
                        {
                            details.ExperienceID = experienceID;
                            details.Name = response.Details.Name;
                            details.Description = response.Details.Description;
                            details.Status = response.Details.Status;
                            if (response.Details.Traffic.HasValue)
                                details.Traffic = Convert.ToDecimal(response.Details.Traffic.Value);
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

        public static bool UpdateExperienceModifiers(int experienceId, int pool, int profile)
        {
            var isSuccesful = false;
            using (_certonaService = new CertonaServiceClient())
            {
                try
                {
                    UserDTO user = FormsAuthenticationWrapper.User;
                    var request = new UpdateExperienceModifiersRequest
                    {
                        User = user,
                        ExperienceID = experienceId,
                        Pool = pool,
                        Profile = profile
                    };
                    var response = _certonaService.UpdateExperienceModifiers(request);

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

        public static ExperienceBoosts GetExperienceBoosts(int experienceID)
        {
            ExperienceBoosts details = null;
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var experienceBoostsRequest = new GetExperienceBoostsRequest() { User = user, ExperienceID = experienceID };
                        var experienceBoostsResponse = _certonaService.GetExperienceBoosts(experienceBoostsRequest);

                        if (experienceBoostsResponse.Success && experienceBoostsResponse.ExperienceDetails != null)
                        {
                            details = new ExperienceBoosts()
                            {
                                ExperienceID = experienceID
                            };

                            details.ExperienceDetails = new ExperienceDetails();
                            details.ExperienceDetails.Pool = experienceBoostsResponse.ExperienceDetails.Pool;
                            details.ExperienceDetails.Profile = experienceBoostsResponse.ExperienceDetails.Profile;

                            /*
                            details.ExperienceBiases = new List<ExperienceBiases>();
                            details.ExperienceBiases.AddRange(experienceBoostsResponse.ExperienceBiases.Select(b => new ExperienceBiases()
                            {
                                BiasID = b.BiasID,
                                ListName = b.ListName,
                                Weight = b.Weight * 100 // Convert decimal value to integer
                            }).ToList());

                            details.ExperienceModels = new List<Model>();
                            details.ExperienceModels.AddRange(experienceBoostsResponse.ExperienceModels.Select(m => new Model()
                            {
                                CatalogID = m.CatalogID,
                                Description = m.Description,
                                ModelID = m.ModelID,
                                Name = m.Name,
                                Weight = m.Weight * 100 // Convert decimal value to integer
                            }));
                             * */
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

        public static List<ExperienceStrategy> GetExperienceStrategies(int experienceID)
        {
            List<ExperienceStrategy> strategies = new List<ExperienceStrategy>();
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var request = new GetExperienceVariantsRequest() { User = user, ExperienceID = experienceID };
                        var response = _certonaService.GetExperienceVariants(request);

                        if (response.Success && response.Variants != null)
                        {
                            strategies.AddRange(response.Variants.Select(x => new ExperienceStrategy()
                                                              {
                                                                  StrategyID = x.StrategyID,
                                                                  Catalog = x.Catalog,
                                                                  CatalogImage = x.CatalogImage,
                                                                  Name = x.Name,
                                                                  Impressions = x.Impressions,
                                                                  PersonalizationType = x.PersonalizationType,
                                                                  Priority = x.Priority,
                                                                  Rules = x.Rules.HasValue ? (x.Rules.Value ? "Y" : null) : null,
                                                                  Status = x.Status,
                                                                  NumberOfItems = x.NumberOfItems,
                                                                  MinItemsReturned = x.MinItemsReturned,
                                                                  BreakOnMinItemsReturned = x.BreakOnMinItemsReturned
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

            return strategies;
        }

        public static int CreateExperience(ExperienceNodeViewModel model)
        {
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var request = new CreateExperienceRequest()
                        {
                            User = user,
                            SchemeID = model.SchemeID,
                            Name = model.Name,
                            Description = model.Description
                        };
                        var response = _certonaService.CreateExperience(request);
                        return response.ExperienceID;
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

        public static void UpdateExperienceDetails(ExperienceNodeViewModel experience)
        {
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var request = new UpdateExperienceDetailsRequest
                        {
                            User = user,
                            ExperienceID = experience.ExperienceID,
                            Name = experience.Name,
                            Description = experience.Description,
                        };
                        _certonaService.UpdateExperienceDetails(request);
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

        public static void UpdateExperienceDetails(SchemeExperience experience)
        {
            try
            {
                ExperienceNodeViewModel exp = GetExperienceDetails(experience.ExperienceID);
                // Get existing                         
                bool activating = (exp.Status != "Active" && experience.Status == "Active");
                bool deactivating = (exp.Status == "Active" && experience.Status != "Active");

                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var request = new UpdateExperienceDetailsRequest
                                            {                                                                         
                                                User = user,
                                                ExperienceID = experience.ExperienceID,                                                
                                                Name = experience.Name,
                                                Description = experience.Description,
                                                Status = experience.Status,                                                                        
                                            };
                        _certonaService.UpdateExperienceDetails(request);                    
                        
                        if (activating)
                        {
                            // for Active (current) one, set Traffic = 100
                            var req = new UpdateExperienceTrafficRequest(){User = user, ExperienceID = experience.ExperienceID};
                            if (experience.Traffic.HasValue)
                            {
                                req.SamplingRangeBegin = 0;
                                req.SamplingRangeEnd = experience.Traffic.Value;
                            }
                            _certonaService.UpdateExperienceTraffic(req);
                        }
                        if (deactivating)
                        {
                            var req = new UpdateExperienceTrafficRequest() { User = user, ExperienceID = experience.ExperienceID };
                            _certonaService.UpdateExperienceTraffic(req); 
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

                if (activating)
                {
                    // set others to Inactive and clear Traffic 
                    DeactivateExperiences(experience);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void DeactivateExperiences(SchemeExperience experience)
        {
            // get all experiences for container
            var m = SchemeModel.GetSchemeExperiences(experience.SchemeID);
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        foreach (var exp in m.Experiences)
                        {
                            if (exp.ExperienceID != experience.ExperienceID && exp.Status == "Active")
                            {
                                // set others to Inactive 
                                var request = new UpdateExperienceDetailsRequest
                                {
                                    User = user,
                                    ExperienceID = exp.ExperienceID,
                                    Status = "Inactive",
                                };
                                _certonaService.UpdateExperienceDetails(request);

                                // clear Traffic
                                var req = new UpdateExperienceTrafficRequest() { User = user, ExperienceID = exp.ExperienceID };
                                _certonaService.UpdateExperienceTraffic(req);
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
                throw;
            }
        }
    }
}