using System;
using System.ServiceModel;
using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.FormAuthentication;
using Resonance.Insight3.Web.ViewModels.Remarketing;

namespace Resonance.Insight3.Web.Models
{
    public class RemarketingModel : ModelBase
    {
        public static RemarketingCampaignViewModel GetRemarketingCampaignViewModel(int remarketingCampaignID)
        {
            RemarketingCampaignViewModel vm = new RemarketingCampaignViewModel(); 
            
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var request = new GetRemarketingCampaignDetailsRequest() { RemarketingCampaignID = remarketingCampaignID, User = FormsAuthenticationWrapper.User };

                        var response = _certonaService.GetRemarketingCampaignDetails(request);
                        if (response.Success && response.RemarketingCampaign != null)
                        {
                            //vm = new RemarketingCampaignViewModel();
                            vm.ApplicationId = response.RemarketingCampaign.Application_ID;
                            vm.RemarketingCampaignId = response.RemarketingCampaign.Remarketing_Campaign_ID;
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

        public static RemarketingCampaignViewModel GetNewRemarketingCampaignViewModel()
        {
            return new RemarketingCampaignViewModel();
        }
    }
}