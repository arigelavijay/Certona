using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Resonance.Insight3.Web.ViewModels.Scheme
{
    public class SchemeEventContext
    {
        public string EventName;
        public bool? DisplayValue = false;
        public int DisplayOrder = int.MaxValue;
    }
}