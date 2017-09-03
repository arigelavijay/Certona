using System;

namespace Resonance.Insight3.Web.ViewModels.Experience
{
    public class Model
    {
        private double? _weight;

        public string CatalogID { get; set; }
        public string Description { get; set; }
        public int ModelID { get; set; }
        public string Name { get; set; }
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