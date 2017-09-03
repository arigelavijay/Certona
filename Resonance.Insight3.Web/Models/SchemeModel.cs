using System;
using System.Linq;
using System.ServiceModel;
using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.FormAuthentication;
using Resonance.Insight3.Web.ViewModels.Scheme;

namespace Resonance.Insight3.Web.Models
{
    public class SchemeModel : ModelBase
    {
        public static SchemeDetails GetSchemeDetails(int schemeID)
        {
            SchemeDetails SchemeDetails = null;
            try
             {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var appDetailRequest = new GetSchemeDetailsRequest {SchemeID = schemeID, User = user};
                        var appDetailResponse = _certonaService.GetSchemeDetails(appDetailRequest);

                        if (appDetailResponse.Success && appDetailResponse.SchemeDetails != null)
                        {
                            SchemeDetails = new SchemeDetails
                                                {
                                                    ApplicationID = appDetailResponse.SchemeDetails.ApplicationID,
                                                    Name = appDetailResponse.SchemeDetails.Name,
                                                    Description = appDetailResponse.SchemeDetails.Description,
                                                    Scheme = appDetailResponse.SchemeDetails.Scheme,
                                                    SchemeType = appDetailResponse.SchemeDetails.SchemeType,
                                                    DefaultCatalogID = appDetailResponse.SchemeDetails.DefaultCatalogID,
                                                    Status = appDetailResponse.SchemeDetails.Status.ToString(),
                                                    RecMethod = appDetailResponse.SchemeDetails.RecMethod,
                                                    Callback = appDetailResponse.SchemeDetails.Callback,
                                                    CustomQueryString =
                                                        appDetailResponse.SchemeDetails.CustomQueryString,
                                                    NumberOfItems = appDetailResponse.SchemeDetails.NumberOfItems,
                                                    Icon_Filename = appDetailResponse.SchemeDetails.Icon_Filename
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

            return SchemeDetails;
        }

        public static SchemeExperienceViewModel GetSchemeExperiences(int schemeID)
        {
            SchemeExperienceViewModel vm = new SchemeExperienceViewModel();
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var getRequest = new GetSchemeExperiencesRequest() { SchemeID = schemeID, User = user };
                        var getResponse = _certonaService.GetSchemeExperiences(getRequest);

                        if (getResponse.Success && getResponse.Experiences != null)
                        {
                            foreach (var dto in getResponse.Experiences)
                            {
                                vm.ContainerID = schemeID;    
                                vm.Experiences.Add(new SchemeExperience()
                                                       {
                                                           SchemeID = schemeID,
                                                           ExperienceID = dto.ExperienceID,
                                                           Name = dto.Name,
                                                           Description = dto.Description,
                                                           Status = dto.Status,
                                                           Traffic = dto.Traffic
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

            return vm;
        }

        public static SchemeContextViewModel GetSchemeContext(int schemeID)
        {
            SchemeContextViewModel vm = new SchemeContextViewModel();
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var getRequest = new GetSchemeContextRequest() { SchemeID = schemeID, User = user };
                        var getResponse = _certonaService.GetSchemeContext(getRequest);

                        if (getResponse.Success && getResponse.SchemeEventsContexts != null)
                        {
                            foreach (var dto in getResponse.SchemeEventsContexts)
                            {
                                if (dto.Name == "Current Product")
                                {
                                    vm.InclusionCriteria.IncludeCurrentAssets = dto.SourceObject;
                                    vm.InclusionCriteria.IncludeMaxCount = dto.SourceObjectMaxCount;
                                    vm.InclusionCriteria.IncludeDaysPast = dto.SourceObjectDaysPast;
                                    vm.InclusionCriteria.IncludeLastPages = dto.SourceObjectLastPages;
                                    // this data comes across on both, so last one wins
                                    vm.RulesCriteria.RulesDaysPast = dto.RulesDaysPast;
                                    vm.RulesCriteria.RulesLastPages = dto.RulesLastPages;
                                    vm.RulesCriteria.RulesMaxCount = dto.RulesMaxCount;
                                }
                                if (dto.Name == "Last Product")
                                {
                                    vm.ExclusionCriteria.ExcludeCurrentAssets = dto.ExcludeItem;
                                    vm.ExclusionCriteria.ExcludeMaxCount = dto.ExcludeItemMaxCount;
                                    vm.ExclusionCriteria.ExcludeDaysPast = dto.ExcludeItemDaysPast;
                                    vm.ExclusionCriteria.ExcludeLastPages = dto.ExcludeItemLastPages;
                                    // this data comes across on both, so last one wins
                                    vm.RulesCriteria.RulesDaysPast = dto.RulesDaysPast;
                                    vm.RulesCriteria.RulesLastPages = dto.RulesLastPages;
                                    vm.RulesCriteria.RulesMaxCount = dto.RulesMaxCount;
                                }
                            }
                        }

                        // 
                        var request = new GetSchemeEventContextRequest() { SchemeID = schemeID, User = user };
                        var response = _certonaService.GetSchemeEventContext(request);
                        if (response.Success && response.SchemeEventsContexts != null)
                        {
                            int counter = 0;
                            foreach(var dto in response.SchemeEventsContexts)
                            {
                                counter++;
                                AddSchemeEventContext(vm, counter,dto );
                            }
                            // The lists should be rendered in order
                            vm.ExclusionCriteria.EventContexts = vm.ExclusionCriteria.EventContexts.OrderBy(r => r.DisplayOrder).ThenBy(r => r.EventName).ToList();
                            vm.InclusionCriteria.EventContexts = vm.InclusionCriteria.EventContexts.OrderBy(r => r.DisplayOrder).ThenBy(r => r.EventName).ToList();
                            vm.RulesCriteria.EventContexts = vm.RulesCriteria.EventContexts.OrderBy(r => r.DisplayOrder).ThenBy(r => r.EventName).ToList();
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

            return vm;
        }

        private static void AddSchemeEventContext(SchemeContextViewModel viewModel, int counter, SchemeEventContextDTO dto)
        {
            SchemeEventContext sec = null;

            // Where we could know that a 1 (true) meant that all events in that event class were selected, 
            // a 0 (false) meant that none of the events in that class were selected, 
            // and -1 (null) meant that some but not all events in that class were selected.

            // Inclusion
            sec = viewModel.InclusionCriteria.EventContexts.FirstOrDefault(f => f.EventName == dto.Name);
            if (sec != null)
            {
                if (dto.SubjectCount.HasValue && dto.Count.HasValue)
                {
                    if (dto.SubjectCount.Value == 0) 
                        sec.DisplayValue = false;
                    else
                    {
                        sec.DisplayValue = null;
                        if (dto.SubjectCount.Value == dto.Count.Value)
                            sec.DisplayValue = true;
                    }                    
                    
                }
                sec.DisplayOrder = counter;
            }

            // Exclusion
            sec = viewModel.ExclusionCriteria.EventContexts.FirstOrDefault(f => f.EventName == dto.Name);
            if (sec != null)
            {
                if (dto.ExcludeCount.HasValue && dto.Count.HasValue)
                {
                    if (dto.ExcludeCount.Value == 0)
                        sec.DisplayValue = false;
                    else
                    {
                        sec.DisplayValue = null;
                        if (dto.ExcludeCount.Value == dto.Count.Value)
                            sec.DisplayValue = true;
                    }                    
                }
                sec.DisplayOrder = counter;                
            }

            // Rules
            sec = viewModel.RulesCriteria.EventContexts.FirstOrDefault(f => f.EventName == dto.Name);
            if (sec != null)
            {
                if (dto.RuleCount.HasValue && dto.Count.HasValue)
                {
                    if (dto.RuleCount.Value == 0)
                        sec.DisplayValue = false;
                    else
                    {
                        sec.DisplayValue = null;
                        if (dto.RuleCount.Value == dto.Count.Value)
                            sec.DisplayValue = true;
                    }
                }
                sec.DisplayOrder = counter;
            }
        }
    }
}
