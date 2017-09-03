using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Resonance.Insight3.Web.ViewModels.Rule
{
    public class ExpressionViewModel
    {
        public int ExpressionID { get; set; }
        public int VariantID { get; set; }
        public int RuleID { get; set; }
        public string RuleName { get; set; }
        public int ConditionID { get; set; }

        [Required(ErrorMessage = "Please select a Attribute")]
        public string Attribute { get; set; }

        [Required(ErrorMessage = "Please select a Operator")]
        public string Operator { get; set; }

        [Required(ErrorMessage = "Please enter a Value")]
        public string Value { get; set; }

        [Required(ErrorMessage = "Please enter a Context")]
        public string Context { get; set; }

        public string PercentOrUnits { get; set; }
        public string PlusOrMinus { get; set; }

        public List<SelectListItem> PlusOrMinusList
        {
            get
            {
                var plusOrMinusList = new List<SelectListItem>();
                plusOrMinusList.Add(new SelectListItem() { Text = "+", Value = "plus" });
                plusOrMinusList.Add(new SelectListItem() { Text = "-", Value = "minus" });
                return plusOrMinusList;
            }
        }

        public List<SelectListItem> PercentOrUnitsList
        {
            get
            {
                var percentOrUnitsList = new List<SelectListItem>();
                percentOrUnitsList.Add(new SelectListItem() { Text = "%", Value = "percent" });
                percentOrUnitsList.Add(new SelectListItem() { Text = "Unit", Value = "units" });
                return percentOrUnitsList;
            }
        }
    }
}