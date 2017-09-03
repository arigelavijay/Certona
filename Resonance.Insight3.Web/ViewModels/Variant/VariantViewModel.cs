using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Resonance.Insight.Web.Bytes.ViewModels.Variant
{
    public class VariantViewModel
    {
        public List<ExperienceBiases> ExperienceBiases { get; set; }
        public ExperienceDetails ExperienceDetails { get; set; }
        public List<Model> ExperienceModel { get; set; }
    }
}