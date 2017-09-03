using System.Collections.Generic;
using System.Web.Mvc;
using Resonance.Insight3.Web.CertonaService;

namespace Resonance.Insight3.Web.ViewModels
{
    /// <summary>
    ///     Navigaiton View Model
    /// </summary>
    public class NavigationViewModel
    {
        /// <summary>
        ///     Account list
        /// </summary>
        public SelectList AccountList { get; set; }

        /// <summary>
        ///     Panel bar width
        /// </summary>
        public decimal? PanelBarWidth { get; set; }

        /// <summary>
        ///     Account explorer status
        /// </summary>
        public List<string> AccountExplorerStatus { get; set; }

        /// <summary>
        ///     Certona credential
        /// </summary>
        public UserDTO User { get; set; }

        /// <summary>
        ///     Error message
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     Help text
        /// </summary>
        public string HelpText { get; set; }

        /// <summary>
        ///     Panelbar states
        /// </summary>
        public List<int> PanelbarStates { get; set; }
    }
}