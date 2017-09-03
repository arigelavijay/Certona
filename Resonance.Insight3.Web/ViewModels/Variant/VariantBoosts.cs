using System.Collections.Generic;

namespace Resonance.Insight3.Web.ViewModels.Variant
{
    public class VariantBoosts
    {
        public int VariantID { get; set; }
        public List<VariantBiases> VariantBiases { get; set; }
        public VariantDetails VariantDetails { get; set; }
        public List<VariantModel> VariantModels { get; set; }
    }
}