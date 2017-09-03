using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Elmah;
using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.FormAuthentication;
using Resonance.Insight3.Web.Helpers;
using Resonance.Insight3.Web.ViewModels;

namespace Resonance.Insight3.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        /// <summary>
        ///     Creates new instance of the service
        /// </summary>
        private ICertonaService foresightService;

        public ICertonaService ForesightService
        {
            set { foresightService = value; } // Dependancy Injection - Called from test framework only 
            get
            {
                if (foresightService == null)
                    foresightService = new CertonaServiceClient();
                return foresightService;
            }
        }

        /// <summary>
        ///     Home view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Home()
        {
            var model = new HomeViewModel {NavigationModel = GetNavigationViewModel()};
            return View("Home", model);
        }

        [HttpGet]
        public PartialViewResult UnsavedChanges()
        {
            return PartialView();
        }

        [HttpGet]
        public void UpdateLastAccountID(string accountID)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(accountID))
                {
                    UserDTO user = FormsAuthenticationWrapper.User;
                    user.LastAccountID = accountID;
                    FormsAuthenticationWrapper.User = user;

                    var setUserSettingsRequest = new SetUserSettingsRequest
                    {
                        User = user,
                        ApplicationID = null,
                        CurrentPassword = null,
                        NewPassword = null,
                        EmailAddress = null
                    };

                    ForesightService.SetUserSettings(setUserSettingsRequest);
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.Logger.WriteException(ex);
            }
        }

        /// <summary>
        ///     Left navigaton panel partial view
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult NavigationPanel(string accountID)
        {
            try
            {
                if (accountID != null && accountID != string.Empty)
                {
                    UserDTO user = FormsAuthenticationWrapper.User;
                    user.LastAccountID = accountID;
                    FormsAuthenticationWrapper.User = user;

                    var setUserSettingsRequest = new SetUserSettingsRequest
                        {
                            User = user,
                            ApplicationID = null,
                            CurrentPassword = null,
                            NewPassword = null,
                            EmailAddress = null
                        };

                    ForesightService.SetUserSettings(setUserSettingsRequest);
                }
                return PartialView("_NavigationPanel", GetNavigationViewModel());
            }
            catch (Exception ex)
            {
                LoggingHelper.Logger.WriteException(ex);
                return PartialView("_NavigationPanel", new NavigationViewModel {ErrorMessage = ex.Message});
            }
        }

        /// <summary>
        ///     Content partial view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult ContentPanel()
        {
            return PartialView("_ContentPanel");
        }

        /// <summary>
        ///     Get navigation panel model
        /// </summary>
        /// <returns>Navigation view model</returns>
        private NavigationViewModel GetNavigationViewModel()
        {
            try
            {
                var user = FormsAuthenticationWrapper.User;

                // Panelbar width
                var panelBarWidthRequest = new GetPanelBarWidthRequest {User = user};
                var panelBarWidthResponse = ForesightService.GetPanelBarWidth(panelBarWidthRequest);

                // Accont explorer status list          
                var accountExplorerStatusRequest = new GetAccountExplorerStatusRequest {User = user};
                var accountExplorerStatusResponse = ForesightService.GetAccountExplorerStatusList(accountExplorerStatusRequest);

                // Panelbar states        
                var panelBarStatesRequest = new GetPanelBarStatesRequest {User = user};
                var panelBarStatesResponse = ForesightService.GetPanelBarStates(panelBarStatesRequest);
                var panelBarStates = panelBarStatesResponse.ExpandedPanelBars.Select(pbs => pbs.PanelBarID);

                return new NavigationViewModel
                    {
                        AccountList = new SelectList(user.Accounts.OrderBy(a => a.AccountID).Select(a => a.AccountID).Distinct(StringComparer.CurrentCultureIgnoreCase).ToList(),
                                           user != null ? user.LastAccountID : string.Empty),
                        PanelBarWidth = panelBarWidthResponse.Width,
                        AccountExplorerStatus = accountExplorerStatusResponse.StatusValues.ToList(),
                        PanelbarStates = panelBarStates.ToList(),
                        //HelpText = helpTextResponse.HelpText,
                        User = user
                    };
            }
            catch (Exception ex)
            {
                LoggingHelper.Logger.WriteException(ex);
                return new NavigationViewModel {ErrorMessage = ex.Message};
            }
        }

        /// <summary>
        ///     Set account explorer state
        /// </summary>
        /// <param name="strStatusValue">strStatusValue</param>
        /// <returns>return partial view</returns>
        [HttpPost]
        public void SetAccountExplorerStatus(string strStatusValue)
        {
            try
            {
                strStatusValue = strStatusValue.Replace("chk", "");

                UserDTO user = FormsAuthenticationWrapper.User;
                var accountExplorerStatusRequest = new SetAccountExplorerStatusRequest
                    {
                        User = user,
                        StatusValue = strStatusValue
                    };
                ForesightService.SetAccountExplorerStatusList(accountExplorerStatusRequest);
            }
            catch (Exception ex)
            {
                LoggingHelper.Logger.WriteException(ex);
            }
        }

        /// <summary>
        ///     Get accont tree view when account status changed
        /// </summary>
        /// <returns>Account tree view partial view</returns>
        [HttpGet]
        public PartialViewResult GetAccountTreeview()
        {
            try
            {
                UserDTO user = FormsAuthenticationWrapper.User;
                var accountExplorerTreeRequest = new GetAccountExplorerTreeRequest {User = user};
                GetAccountExplorerTreeResponse accountExplorerTreeResponse =
                    ForesightService.GetAccountExplorerTree(accountExplorerTreeRequest);

                return PartialView("_AccountTreeView", accountExplorerTreeResponse.Nodes.ToList());
            }
            catch (Exception ex)
            {
                LoggingHelper.Logger.WriteException(ex);
                return PartialView("_AccountTreeView", new List<NodeDTO>());
            }
        }

        private NodeDTO GetSelectedNode(List<NodeDTO> nodes)
        {
            NodeDTO selected = null;
            foreach (NodeDTO node in nodes)
            {
                if (node.Selected)
                {
                    return node;
                }

                selected = GetSelectedNode(node.ChildNodes);

                if (selected != null)
                {
                    break;
                }
            }

            return selected;
        }
    }
}