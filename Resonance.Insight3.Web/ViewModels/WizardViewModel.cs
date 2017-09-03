using System.Collections.Generic;
using Resonance.Insight3.Web.CertonaService;

namespace Resonance.Insight3.Web.ViewModels
{
    public class WizardViewModel : IViewModel
    {
        public WizardViewModel()
        {
            ContentPanelSections = new List<ContentPanelSectionDTO>();
        }

        public string ContentPanelName { get; set; }
        public List<ContentPanelSectionDTO> ContentPanelSections { get; set; }
        public string NodeID { get; set; }
        public string NodeType { get; set; }
        public string ApplicationID { get; set; }

        public string WizardMode { get; set; }
    }
}