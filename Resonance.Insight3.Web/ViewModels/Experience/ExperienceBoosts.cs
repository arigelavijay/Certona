using System.Collections.Generic;

namespace Resonance.Insight3.Web.ViewModels.Experience
{
    public class ExperienceBoosts
    {
        public int ExperienceID { get; set; }
        public List<ExperienceBiases> ExperienceBiases { get; set; }
        public ExperienceDetails ExperienceDetails { get; set; }
        public List<Model> ExperienceModels { get; set; }
    }
}