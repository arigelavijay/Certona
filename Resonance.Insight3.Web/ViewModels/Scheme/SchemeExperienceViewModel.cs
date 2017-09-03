using System.Collections.Generic;

namespace Resonance.Insight3.Web.ViewModels.Scheme
{
    public class SchemeExperienceViewModel
    {
        public SchemeExperienceViewModel()
        {
            Experiences = new List<SchemeExperience>();
        }

        public List<SchemeExperience> Experiences { get; set; }
        public int ContainerID { get; set; } // parent SchemeID
        public int? CreatedExperienceId { get; set; } // newly created experience id (via CreateExperiences dialog)
        public string CreatedExperienceName { get; set; }
    }
}