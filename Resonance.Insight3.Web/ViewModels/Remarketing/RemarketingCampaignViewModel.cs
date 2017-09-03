namespace Resonance.Insight3.Web.ViewModels.Remarketing
{
    public class RemarketingCampaignViewModel
    {
        public string AccountId { get; set; }
        public string ApplicationId { get; set; }
        public int RemarketingCampaignId { get; set; }
        public string CurrentStep { get; set; }
        public string Status { get; set; }
        public string Errors { get; set; }
        public bool RulesEnabled { get; set; }
    }
}