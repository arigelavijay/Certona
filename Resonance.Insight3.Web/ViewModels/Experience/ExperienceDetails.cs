using System;

namespace Resonance.Insight3.Web.ViewModels.Experience
{
    public class ExperienceDetails
    {
        private int _pool;

        public int Pool { get; set; }
        
        public int PoolMultipliedByTen
        {
            get
            {
                if (Pool >= 10)
                {
                    _pool = 10;
                }
                else if (Pool == 0)
                {
                    _pool = 0;
                }
                else
                {
                    _pool = Pool*10;
                }

                return _pool;
            }

            set { _pool = value;  }
        }

        public int Profile { get; set; }
    }
}