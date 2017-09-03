using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Resonance.Insight3.Web.CertonaService;

namespace Resonance.Insight3.Web.ViewModels.Catalog
{
    public class GetAssetsModelResponse
    {
        public GetAssetsModelResponse()
        {
            Items = new List<CatalogItemInfoDTO>();
        }

        public List<CatalogItemInfoDTO> Items { get; set; }
        public int Total { get; set; }
    }
}