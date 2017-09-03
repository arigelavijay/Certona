using System.Collections.Generic;
using System.Data;
using Resonance.Insight3.Web.CertonaService;

namespace Resonance.Insight3.Web.ViewModels.Models
{
    public class ModelDataViewModel
    {
        public ModelDataViewModel()
        {
            GridData = new DataTable();
            Errors = new List<ErrorResult>();
        }

        public DataTable GridData;
        public string[] ColumnGroupHeaders { get; set; }
        public string ModelName { get; set; }
        public string CatalogName { get; set; }
        public int ModelID { get; set; }
        public List<ErrorResult> Errors { get; set; }
    }
}