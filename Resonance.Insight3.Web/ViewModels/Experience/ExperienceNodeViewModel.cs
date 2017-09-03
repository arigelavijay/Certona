using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Resonance.Insight3.Web.ViewModels.Variant;

namespace Resonance.Insight3.Web.ViewModels.Experience
{
    public class ExperienceNodeViewModel
    {
        public int SchemeID { get; set; }
        public int ExperienceID { get; set; }
        public int? CreatedVariantId { get; set; } // newly created Strategy(Variant) id (via CreateStrategies dialog)
        
        [Required()]
        [StringLength(50, ErrorMessage = "Must be less than 50 characters")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [StringLength(255, ErrorMessage = "Must be less than 255 characters")]
        public string Description { get; set; }

        public string Status { get; set; }
        public decimal? Traffic { get; set; }
        public int StrategyCount { get; set; }

        // Added to allow for local data binding
        public List<int> StrategyIDList { get; set; }
        public List<VariantRule> VariantRules { get; set; }
        public List<ExperienceStrategyDecisionPlan> DecisionPlans { get; set; }
        public List<SelectListItem> Priority { get; set; }
        public List<SelectListItem> NumberOfAssets { get; set; }

        public bool ShowRules
        {
            get
            {
                if (HttpContext.Current.Session["Strategies.ShowRules"] == null)
                {
                    HttpContext.Current.Session["Strategies.ShowRules"] = true;
                }
                return (bool)HttpContext.Current.Session["Strategies.ShowRules"];
            }
            set
            {
                HttpContext.Current.Session["Strategies.ShowRules"] = value;
            }
        }

        public bool ShowDecisionPlan
        {
            get
            {
                if (HttpContext.Current.Session["Strategies.ShowDecisionPlan"] == null)
                {
                    HttpContext.Current.Session["Strategies.ShowDecisionPlan"] = true;
                }
                return (bool)HttpContext.Current.Session["Strategies.ShowDecisionPlan"];
            }
            set
            {
                HttpContext.Current.Session["Strategies.ShowDecisionPlan"] = value;
            }
        }
    }
}