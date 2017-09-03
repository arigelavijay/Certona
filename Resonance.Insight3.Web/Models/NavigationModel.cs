using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.FormAuthentication;
using Resonance.Insight3.Web.HtmlHelpers;
using Resonance.Insight3.Web.ViewModels.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.FormAuthentication;
using Resonance.Insight3.Web.Helpers;

namespace Resonance.Insight3.Web.Models
{
    public class NavigationModel : ModelBase
    {
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

        public static void SetSelectedNode(string nodeID, string nodeType, int? classificationID)
        {
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var selectedNodeRequest = new SetSelectedNodeRequest
                        {
                            User = user,
                            ObjectKey = nodeType,
                            ObjectValue = nodeID
                        };

                        if (classificationID.HasValue)
                        {
                            selectedNodeRequest.ClassificationID = classificationID.Value;
                        }

                        _certonaService.SetSelectedNode(selectedNodeRequest);

                    }
                    catch (TimeoutException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                    catch (CommunicationException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void SetAccountExplorerNodeState(string nodeID, string nodeType, string nodeName, int? classificationID, bool expand)
        {
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var nodeDTO = new NodeDTO
                        {
                            NodeID = nodeID,
                            DisplayValue = nodeName,
                            NodeType = (NodeType)Enum.Parse(typeof(NodeType), nodeType),
                            ClassificationID = classificationID,
                            Expanded = expand
                        };

                        var accountExplorerNodeStateRequest = new SetAccountExplorerNodeStateRequest
                        {
                            User = user,
                            Node = nodeDTO
                        };

                        _certonaService.SetAccountExplorerNodeState(accountExplorerNodeStateRequest);
                    }
                    catch (TimeoutException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                    catch (CommunicationException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void SetAssetExplorerNodeState(string nodeID, string nodeType, string nodeName, bool expand)
        {
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var nodeDTO = new NodeDTO
                        {
                            NodeID = nodeID,
                            DisplayValue = nodeName,
                            NodeType = (NodeType)Enum.Parse(typeof(NodeType), nodeType),
                            Expanded = expand
                        };
                        var assetExplorerNodeStateRequest = new SetAssetExplorerNodeStateRequest
                        {
                            User = user,
                            Node = nodeDTO
                        };
                        _certonaService.SetAssetExplorerNodeState(assetExplorerNodeStateRequest);
                    }
                    catch (TimeoutException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                    catch (CommunicationException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void SetPanelBarWidth(decimal width)
        {
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var panelBarWidthRequest = new SetPanelBarWidthRequest
                        {
                            User = user,
                            Width = width
                        };
                        _certonaService.SetPanelBarWidth(panelBarWidthRequest);
                    }
                    catch (TimeoutException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                    catch (CommunicationException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void SetPanelBarStates(int panelBarId)
        {
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var panelBarDTO = new List<PanelBarDTO> { new PanelBarDTO { PanelBarID = panelBarId } };
                        var panelBarStatesRequest = new SetPanelBarStatesRequest
                        {
                            User = user,
                            PanelBars = panelBarDTO
                        };
                        _certonaService.SetPanelBarStates(panelBarStatesRequest);
                    }
                    catch (TimeoutException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                    catch (CommunicationException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static List<NodeDTO> GetReportTreeItems()
        {
            var nodes = new List<NodeDTO>();

            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        var user = FormsAuthenticationWrapper.User;
                        var reportsListRequest = new GetReportsListRequest { User = user };
                        var reportsListResponse = _certonaService.GetReportsList(reportsListRequest);
                        var reportNodes = reportsListResponse.Reports.Select(r => new NodeDTO
                        {
                            DisplayValue = r.ReportName,
                            NodeID = r.ReportID.ToString(),
                            NodeType = NodeType.Report,
                            Selected = r.Selected
                        });

                        return reportNodes.ToList();
                    }
                    catch (TimeoutException exception)
                    {
                        _certonaService.Abort();
                    }
                    catch (CommunicationException exception)
                    {
                        _certonaService.Abort();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return nodes;
        }

        public static List<NodeDTO> GetAccountTreeItems()
        {
            var nodes = new List<NodeDTO>();

            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        var user = FormsAuthenticationWrapper.User;
                        var accountExplorerTreeRequest = new GetAccountExplorerTreeRequest { User = user };
                        var accountExplorerTreeResponse = _certonaService.GetAccountExplorerTree(accountExplorerTreeRequest);

                        return accountExplorerTreeResponse.Nodes.ToList();
                    }
                    catch (TimeoutException exception)
                    {
                        _certonaService.Abort();
                    }
                    catch (CommunicationException exception)
                    {
                        _certonaService.Abort();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return nodes;
        }

        public static List<NodeDTO> GetAssetTreeItems()
        {
           var nodes = new List<NodeDTO>();

            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        var user = FormsAuthenticationWrapper.User;
                        var assetExplorerTreeRequest = new GetAssetExplorerTreeRequest { User = user };
                        var assetExplorerTreeResponse = _certonaService.GetAssetExplorerTree(assetExplorerTreeRequest);

                        return assetExplorerTreeResponse.Nodes.ToList();
                    }
                    catch (TimeoutException exception)
                    {
                        _certonaService.Abort();
                    }
                    catch (CommunicationException exception)
                    {
                        _certonaService.Abort();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return nodes;
        }

        public static List<NodeDTO> GetToolTreeItems()
        {
           var nodes = new List<NodeDTO>();

            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        var user = FormsAuthenticationWrapper.User;
                        var toolsListRequest = new GetToolsListRequest { User = user };
                        var toolsResponse = _certonaService.GetToolsList(toolsListRequest);
                        var toolNodes = toolsResponse.Tools.Select(t => new NodeDTO
                        {
                            DisplayValue = t.ToolName,
                            NodeID = t.ToolID.ToString(),
                            NodeType = NodeType.Tool,
                            Selected = t.Selected
                        });

                        return toolNodes.ToList();
                    }
                    catch (TimeoutException exception)
                    {
                        _certonaService.Abort();
                    }
                    catch (CommunicationException exception)
                    {
                        _certonaService.Abort();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return nodes;
        }

        [Serializable]
        public class JavaScriptErrorException : Exception
        {
            public JavaScriptErrorException(string message)
                : base(message)
            {
            }
        }
    }
}