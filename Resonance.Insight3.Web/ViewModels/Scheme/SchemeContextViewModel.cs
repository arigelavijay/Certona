using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Resonance.Insight3.Web.ViewModels.Scheme
{
    public class SchemeContextViewModel
    {
        public SchemeContextViewModel()
        {
            InclusionCriteria = new SchemeContextInclusion();
            ExclusionCriteria = new SchemeContextExclusion();
            RulesCriteria = new SchemeContextRules();

            InclusionCriteria.IncludeCurrentAssets = false;
            InclusionCriteria.IncludePreviousAssets = false;
            ExclusionCriteria.ExcludeCurrentAssets = false;
            ExclusionCriteria.ExcludePreviousAssets = false;

            /* Event Classes come from ResConfig
            Other
            Recommendation
            Product View
            Shopping Cart
            Purchase Confirmation
            Content View
            Search
            Link
            */

            var eventClass = new SchemeEventContext() {EventName = "Other"};
            InclusionCriteria.EventContexts.Add(eventClass);
            eventClass = new SchemeEventContext() { EventName = "Recommendation" };
            InclusionCriteria.EventContexts.Add(eventClass);
            eventClass = new SchemeEventContext() { EventName = "Product View" };
            InclusionCriteria.EventContexts.Add(eventClass);
            eventClass = new SchemeEventContext() { EventName = "Shopping Cart" };
            InclusionCriteria.EventContexts.Add(eventClass);
            eventClass = new SchemeEventContext() { EventName = "Purchase Confirmation" };
            InclusionCriteria.EventContexts.Add(eventClass);
            eventClass = new SchemeEventContext() { EventName = "Content View" };
            InclusionCriteria.EventContexts.Add(eventClass);
            eventClass = new SchemeEventContext() { EventName = "Search" };
            InclusionCriteria.EventContexts.Add(eventClass);
            eventClass = new SchemeEventContext() { EventName = "Link" };
            InclusionCriteria.EventContexts.Add(eventClass);

            eventClass = new SchemeEventContext() { EventName = "Other" };           
            ExclusionCriteria.EventContexts.Add(eventClass);
            eventClass = new SchemeEventContext() { EventName = "Recommendation" };
            ExclusionCriteria.EventContexts.Add(eventClass);
            eventClass = new SchemeEventContext() { EventName = "Product View" };            
            ExclusionCriteria.EventContexts.Add(eventClass);
            eventClass = new SchemeEventContext() { EventName = "Shopping Cart" };            
            ExclusionCriteria.EventContexts.Add(eventClass);
            eventClass = new SchemeEventContext() { EventName = "Purchase Confirmation" };
            ExclusionCriteria.EventContexts.Add(eventClass);
            eventClass = new SchemeEventContext() { EventName = "Content View" };
            ExclusionCriteria.EventContexts.Add(eventClass);
            eventClass = new SchemeEventContext() { EventName = "Search" };
            ExclusionCriteria.EventContexts.Add(eventClass);
            eventClass = new SchemeEventContext() { EventName = "Link" };
            ExclusionCriteria.EventContexts.Add(eventClass);

            eventClass = new SchemeEventContext() { EventName = "Other" };
            RulesCriteria.EventContexts.Add(eventClass);
            eventClass = new SchemeEventContext() { EventName = "Recommendation" };
            RulesCriteria.EventContexts.Add(eventClass);
            eventClass = new SchemeEventContext() { EventName = "Product View" };
            RulesCriteria.EventContexts.Add(eventClass);
            eventClass = new SchemeEventContext() { EventName = "Shopping Cart" };
            RulesCriteria.EventContexts.Add(eventClass);
            eventClass = new SchemeEventContext() { EventName = "Purchase Confirmation" };
            RulesCriteria.EventContexts.Add(eventClass);
            eventClass = new SchemeEventContext() { EventName = "Content View" };
            RulesCriteria.EventContexts.Add(eventClass);
            eventClass = new SchemeEventContext() { EventName = "Search" };
            RulesCriteria.EventContexts.Add(eventClass);
            eventClass = new SchemeEventContext() { EventName = "Link" };
            RulesCriteria.EventContexts.Add(eventClass);     
        }

        public SchemeContextInclusion InclusionCriteria { get; set; }
        public SchemeContextExclusion ExclusionCriteria { get; set; }
        public SchemeContextRules RulesCriteria { get; set; }
    }
}