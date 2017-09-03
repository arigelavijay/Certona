using System.Collections.Generic;

namespace Resonance.Insight3.Web.ViewModels.Experience
{
    public class ExperienceViewModel
    {
        public List<ExperienceBiases> ExperienceBiases { get; set; }
        public ExperienceDetails ExperienceDetails { get; set; }
        public List<Model> ExperienceModel { get; set; }
    }
}