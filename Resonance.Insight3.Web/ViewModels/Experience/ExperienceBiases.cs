using System;

namespace Resonance.Insight3.Web.ViewModels.Experience
{
    public class ExperienceBiases
    {
        private double? _weight;

        public int BiasID { get; set; }
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

        public string ListName { get; set; }
    }
}