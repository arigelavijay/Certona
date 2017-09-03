using System.Collections.Generic;
using Resonance.Insight3.Web.CertonaService;

namespace Resonance.Insight3.Web.ViewModels.Models
{
    public class SegmentItemsGridViewModel
    {
        public SegmentItemsGridViewModel()
        {
            Errors = new List<ErrorResult>();
            SegmentItems = new List<SegmentItemsViewModel>();
        }

        public string AccountItemIDDisplay;
        public string DescriptionDisplay;
        public int? SegmentID;
        public string SegmentName;
        public int? ModelID;
        public List<SegmentItemsViewModel> SegmentItems { get; set; }
        public List<ErrorResult> Errors { get; set; }
    }
}