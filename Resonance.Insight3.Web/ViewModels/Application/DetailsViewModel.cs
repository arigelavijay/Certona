using System;
using System.Collections.Generic;
using Resonance.Insight3.Web.CertonaService;

namespace Resonance.Insight3.Web.ViewModels.Application
{
    public class ApplicationViewModel
    {
        public ApplicationDetails ApplicationDetails { get; set; }
        public List<ApplicationPackage> ApplicationPackages { get; set; }
        public ApplicationRemarketingCampaigns ApplicationRemarketingCampaigns { get; set; }
    }

    public class ApplicationDetails
    {
        public ApplicationDetails()
        {
            Classifications = new List<ApplicationClassificationsDTO>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string ApplicationID { get; set; }
        public string URL { get; set; }
        public List<ApplicationClassificationsDTO> Classifications { get; set; }
    }

    public class ApplicationPackage
    {
        public Int32 PackageId { get; set; }
        public String PackageName { get; set; }
        public String Status { get; set; }
        public String Type { get; set; }
        public String Description { get; set; }
        public String SubType { get; set; }
    }
    
    public class ApplicationRemarketingCampaigns
    {
        public string ApplicationID { get; set; }
        public bool FrequencyCapEnabled { get; set; }
        public int FrequencyCapDays { get; set; }
        public int FrequencyCapEmails { get; set; }

        public List<RemarketingCampaignSummaryDTO> RemarketingCampaigns { get; set; }
    }
}