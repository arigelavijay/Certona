using System;

namespace Resonance.Insight3.Web.ViewModels.Variant
{
    public class VariantModel
    {
        private double? _weight;

        public string CatalogID;
        public string Description;
        public int ModelID;
        public string Name;
        public double? Weight
        {
            get
            {
                if (_weight.HasValue) 
                {
                    return _weight;
                }

                return 0;
            }

            set { _weight = value; }
        }
    }
}