using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.FormAuthentication;
using Resonance.Insight3.Web.HtmlHelpers;
using Resonance.Insight3.Web.ViewModels.Application;

namespace Resonance.Insight3.Web.Models
{
    public class ApplicationModel : ModelBase
    {
        public static ApplicationDetails GetApplicationDetailsViewModel(string applicationID)
        {
            ApplicationDetails applicationDetails = null;
            try
            {
                using(_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var appDetailRequest = new GetApplicationDetailsRequest { ApplicationID = applicationID, User = user };

                        var appDetailResponse = _certonaService.GetApplicationDetails(appDetailRequest);

                        
                        if (appDetailResponse.Success && appDetailResponse.ApplicationDetails != null)
                        {
                            applicationDetails = new ApplicationDetails
                            {
                                Name = appDetailResponse.ApplicationDetails.Name,
                                Description = appDetailResponse.ApplicationDetails.Description,
                                ApplicationID = applicationID,
                                URL = HtmlExtensions.UrlPrependHttp(appDetailResponse.ApplicationDetails.ApplicationURL),
                                Classifications = appDetailResponse.ApplicationDetails.Classifications
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
            
            
            return applicationDetails;
        }

        public static List<ApplicationPackage> GetApplicationPackagesViewModel(string applicationID)
        {
            var appPackageList = new List<ApplicationPackage>();
            try
            {
                using(_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var appPackagesRequest = new GetApplicationPackagesRequest { ApplicationID = applicationID, User = user };
                        var appPackagesResponse = _certonaService.GetApplicationPackages(appPackagesRequest);


                        if (appPackagesResponse.Success && appPackagesResponse.ApplicationPackages.Count > 0)
                        {
                            appPackageList.AddRange(appPackagesResponse.ApplicationPackages.Select(appPackage => new ApplicationPackage
                                {
                                    PackageId = appPackage.PackageId, PackageName = appPackage.PackageName, Status = appPackage.Status, Type = appPackage.PackageType, Description = appPackage.Description, SubType = appPackage.SubType
                                }));
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
            
            return appPackageList;
        }
        
        public static ApplicationRemarketingCampaigns GetRemarketingCampaignsViewModel(string applicationID, string statusFilter = "All")
        {
            var campaigns = new ApplicationRemarketingCampaigns();
            try
            {
                using(_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        // Remarketing Campaigns
                        var campaignSummaryRequest = new GetApplicationRemarketingCampaignsRequest() { ApplicationID = applicationID, StatusFilter = statusFilter, User = user };
                        var campaignSummaryResponse = _certonaService.GetApplicationRemarketingCampaigns(campaignSummaryRequest);


                        if (campaignSummaryResponse.Success && campaignSummaryResponse.RemarketingCampaigns.Count > 0)
                        {
                            campaigns.RemarketingCampaigns = new List<RemarketingCampaignSummaryDTO>();
                            foreach (var campaign in campaignSummaryResponse.RemarketingCampaigns)
                            {
                                campaigns.RemarketingCampaigns.Add(new RemarketingCampaignSummaryDTO()
                                {
                                    RemarketingCampaignId = campaign.RemarketingCampaignId,
                                    Name = campaign.Name,
                                    Status = campaign.Status,
                                    LastModified = campaign.LastModified,
                                    LastSent = campaign.LastSent,
                                    NextScheduled = campaign.NextScheduled
                                });
                            }
                        }

                        // Frequency Caps
                        var freqCap = GetFrequencyCap(applicationID, user);
                        if (freqCap != null)
                        {
                            campaigns.FrequencyCapEnabled = freqCap.Enabled;
                            campaigns.FrequencyCapDays = freqCap.FrequencyCapDays;
                            campaigns.FrequencyCapEmails = freqCap.FrequencyCapEmails;
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

            return campaigns;   
        }

        public static ApplicationFrequencyCapDTO GetFrequencyCap(string applicationID, UserDTO user)
        {
            var ret = new ApplicationFrequencyCapDTO();
            try
            {
                using(_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        var frequencyCapRequest = new GetApplicationFrequencyCapRequest() { ApplicationID = applicationID, User = user };
                        var frequencyCapResponse = _certonaService.GetApplicationFrequencyCap(frequencyCapRequest);
                        ret = frequencyCapResponse.FrequencyCap;
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

        public static bool SetFrequencyCap(ApplicationFrequencyCapDTO frequencyCap, UserDTO user)
        {
            try
            {
                using(_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        var frequencyCapRequest = new SetApplicationFrequencyCapsRequest() { ApplicationID = frequencyCap.ApplicationID, FrequencyCapDTO = frequencyCap, User = user };
                        _certonaService.SetApplicationFrequencyCap(frequencyCapRequest);
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
            
            return true;
        }


        [Obsolete]
        public static ApplicationViewModel GetApplicationViewModel(NodeType nodeType, string ID)
        {
            // Adding Application Details
            var vm = new ApplicationViewModel();

            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var appDetailResponse = new GetApplicationDetailsResponse();
                        var appDetailRequest = new GetApplicationDetailsRequest { ApplicationID = ID, User = user };
                        appDetailResponse = _certonaService.GetApplicationDetails(appDetailRequest);

                        var applicationDetails = new ApplicationDetails
                        {
                            Name = appDetailResponse.ApplicationDetails.Name,
                            Description = appDetailResponse.ApplicationDetails.Description,
                            ApplicationID = ID,
                            URL = appDetailResponse.ApplicationDetails.ApplicationURL,
                            Classifications = appDetailResponse.ApplicationDetails.Classifications
                        };
                        vm.ApplicationDetails = applicationDetails;

                        // Adding Manage Locations
                        var appPackagesRequest = new GetApplicationPackagesRequest { ApplicationID = ID, User = user };
                        var appPackagesResponse = _certonaService.GetApplicationPackages(appPackagesRequest);

                        List<ApplicationPackage> appPackageList = new List<ApplicationPackage>();
                        if (appPackagesResponse.ApplicationPackages.Count > 0)
                        {
                            foreach (var appPackage in appPackagesResponse.ApplicationPackages)
                            {
                                ApplicationPackage pkg = new ApplicationPackage
                                {
                                    PackageId = appPackage.PackageId,
                                    PackageName = appPackage.PackageName,
                                    Status = appPackage.Status,
                                    Type = appPackage.PackageType,
                                    Description = appPackage.Description,
                                    SubType = appPackage.SubType
                                };
                                appPackageList.Add(pkg);
                            }
                        }

                        vm.ApplicationPackages = appPackageList;


                        // Remarketing Campaigns
                        var campaignSummaryRequest = new GetApplicationRemarketingCampaignsRequest() { ApplicationID = ID, StatusFilter = "All", User = user };
                        var campaignSummaryResponse = _certonaService.GetApplicationRemarketingCampaigns(campaignSummaryRequest);
                        vm.ApplicationRemarketingCampaigns = new ApplicationRemarketingCampaigns();
                        if (campaignSummaryResponse.RemarketingCampaigns.Count > 0)
                        {
                            vm.ApplicationRemarketingCampaigns.RemarketingCampaigns = new List<RemarketingCampaignSummaryDTO>();
                            foreach (var campaign in campaignSummaryResponse.RemarketingCampaigns)
                            {
                                vm.ApplicationRemarketingCampaigns.RemarketingCampaigns.Add(new RemarketingCampaignSummaryDTO()
                                {
                                    RemarketingCampaignId = campaign.RemarketingCampaignId,
                                    Name = campaign.Name,
                                    Status = campaign.Status,
                                    LastModified = campaign.LastModified,
                                    LastSent = campaign.LastSent,
                                    NextScheduled = campaign.NextScheduled
                                });
                            }
                        }

                        // Frequency Caps
                        var freqCap = GetFrequencyCap(ID, user);
                        if (freqCap != null)
                        {
                            vm.ApplicationRemarketingCampaigns.FrequencyCapEnabled = freqCap.Enabled;
                            vm.ApplicationRemarketingCampaigns.FrequencyCapDays = freqCap.FrequencyCapDays;
                            vm.ApplicationRemarketingCampaigns.FrequencyCapEmails = freqCap.FrequencyCapEmails;
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

    }
}