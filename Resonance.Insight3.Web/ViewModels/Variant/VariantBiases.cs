using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resonance.Insight3.Web.ViewModels.Variant
{
    public class VariantBiases
    {
        private double? _weight;

        public int BiasID { get; set; }
        public double? Weight {
            get
            {
                if (_weight.HasValue)
                {

                    return _weight * 100;
                }

                return 0;
            }

            set { _weight = value; }
        }

        public string ListName { get; set; }
    }
}