using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.FormAuthentication;
using Resonance.Insight3.Web.Models;

namespace Resonance.Insight3.Web.ViewModels.Rule
{
    public class RuleViewModel : ModelBase
    {
        public int VariantID { get; set; }

        public int RuleID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Must be less than 100 characters")]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}