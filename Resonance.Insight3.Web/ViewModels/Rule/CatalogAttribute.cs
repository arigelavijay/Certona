using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Resonance.Insight3.Web.Models;
using Resonance.Insight3.Web.CertonaService;

namespace Resonance.Insight3.Web.ViewModels.Rule
{
    public class CatalogAttribute : ModelBase
    {
        public string CatalogName { get; set; }

        public string CatalogID { get; set; }

        public List<CatalogMappingFieldDTO> CatalogMappingFields { get; set; }
    }
}