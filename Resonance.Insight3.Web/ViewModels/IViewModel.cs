using System.Collections.Generic;
using Resonance.Insight3.Web.CertonaService;

namespace Resonance.Insight3.Web.ViewModels
{
    public interface IViewModel
    {
        string ContentPanelName { get; set; }
        List<ContentPanelSectionDTO> ContentPanelSections { get; set; }
        string NodeID { get; set; }
        string NodeType { get; set; }
        string ApplicationID { get; set; }
    }
}