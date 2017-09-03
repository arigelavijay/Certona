using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Resonance.Insight3.Web.Models;

namespace Resonance.Insight3.Web.ViewModels.Variant
{
    public class VariantDetails
    {
        private int _poolFactor ;

        public int VariantId { get; set; }
        public string Catalog { get; set; }
        public string CatalogId { get; set; }
        
        [StringLength(255, ErrorMessage = "Must be less than 255 characters")]
        public string ContainerTitle { get; set; }
        
        [StringLength(255, ErrorMessage = "Must be less than 255 characters")]
        public string Description { get; set; }
        
        public string Icon_FileName { get; set; }
        
        [StringLength(100, ErrorMessage = "Must be less than 100 characters")]
        public string Name { get; set; }
        
        public int NumberOfAssets { get; set; }
        public string PersonalizationType { get; set; }
        public int MethodId { get; set; }
        public int PoolFactor { get; set; }
        public int PoolFactorMultipliedByTen
        {
            get
            {
                if (PoolFactor >= 10)
                {
                    _poolFactor = 10;
                }
                else if (PoolFactor == 0)
                {
                    _poolFactor = 0;
                }
                else
                {
                    _poolFactor = PoolFactor*10;
                }

                return _poolFactor;
            }
        }
        public int Priority { get; set; }
        public string RecommendationType { get; set; }
        public int SubjectWeight { get; set; }
        public string  Status { get; set; }

        public List<SelectListItem> PersonalizationTypeList 
        {
            get
            {
                if (!string.IsNullOrEmpty(CatalogId))
                {
                    var list = Resonance.Insight3.Web.Models.VariantModel.GetRecTypeList(CatalogId);
                    return list.Select(s => new SelectListItem { Text = s.Name, Value = s.MethodID.ToString(), Selected = (s.Name == PersonalizationType) ? true : false }).ToList();         
                }
                return new List<SelectListItem>();
            }
        }

        public List<SelectListItem> StatusList 
        {
            get
            {
                var list = ApplicationModel.GetStatusList();
                return list.Select(s => new SelectListItem {Text = s, Value = s}).ToList();                
            }
        }
    }
}