using System.ComponentModel.DataAnnotations;

namespace Resonance.Insight3.Web.ViewModels.Scheme
{
    public class SchemeExperience
    {
        public int SchemeID { get; set; }
        public int ExperienceID { get; set; }
        
        [Required()]
        [StringLength(25, ErrorMessage = "Must be less than 25 characters")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        
        [StringLength(255, ErrorMessage = "Must be less than 255 characters")]
        public string Description { get; set; }

        public double? Traffic { get; set; }
        public string Status { get; set; }
    }
}