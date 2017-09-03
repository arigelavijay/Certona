using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Resonance.Insight3.Web.ViewModels.Catalog
{
    public class CatalogDetails
    {
        [Required]
        public string CatalogId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(255, ErrorMessage = "Description must be no more than 255 characters.")]
        public string Description { get; set; }
        public string Icon_FileName { get; set; }
        public string AssetType { get; set; }
        public string LanguageName { get; set; }
        public Dictionary<string,int> CatalogApplications { get; set; }
    }
}