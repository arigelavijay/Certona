using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.ViewModels.Sample;

namespace Resonance.Insight3.Web.Models
{
    public class SampleModel : ModelBase
    {
        public static DetailsViewModel GetSampleApplication(NodeType nodeType, string ID)
        {
            var dto = new DetailsViewModel
                {
                    Name = "Sample " + nodeType.ToString(),
                    ID = ID,
                    Description = "A sample description",
                    NodeType = nodeType
                };

            return dto;
        }
    }
}