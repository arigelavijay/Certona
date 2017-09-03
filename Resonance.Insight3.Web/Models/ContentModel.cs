using System;
using System.Collections.Generic;
using System.ServiceModel;
using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.FormAuthentication;

namespace Resonance.Insight3.Web.Models
{
    public class ContentModel : ModelBase
    {
        public static ContentPanelDTO GetSampleContentPanel()
        {
            var cp = new ContentPanelDTO
                {
                    Name = "Sample Panel",
                    PartialViewName = "~/Views/Shared/_ContentTemplate.cshtml",
                    ContentPanelSections = new List<ContentPanelSectionDTO>()
                };

            var cpsList = new List<ContentPanelSectionDTO>();
            cp.ContentPanelSections = cpsList;

            cp.ContentPanelSections.Add(new ContentPanelSectionDTO
                {
                    Name = "Details1",
                    PartialViewName = "~/Views/Sample/Details.cshtml",
                    Expanded = true
                });

            cp.ContentPanelSections.Add(new ContentPanelSectionDTO
                {
                    Name = "Details2",
                    PartialViewName = "~/Views/Sample/Details.cshtml",
                    Expanded = true
                });

            cp.ContentPanelSections.Add(new ContentPanelSectionDTO
                {
                    Name = "Details3",
                    PartialViewName = "~/Views/Sample/Details.cshtml",
                    Expanded = true
                });

            return cp;
        }

        public static ContentPanelDTO GetContentPanel(int nodeTypeID)
        {
            var cp = new ContentPanelDTO();

            try
            {
                using(_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        var request = new GetContentPanelRequest
                        {
                            NodeTypeID = nodeTypeID,
                            User = FormsAuthenticationWrapper.User
                        };

                        var response = _certonaService.GetContentPanel(request);

                        if (response.Success)
                        {
                            cp = response.ContentPanel;
                        }
                        else
                        {
                            cp = new ContentPanelDTO
                            {
                                Name = "Sample Panel",
                                PartialViewName = "~/Views/Shared/_ContentTemplate.cshtml",
                                ContentPanelSections = new List<ContentPanelSectionDTO>()
                            };

                            var cpsList = new List<ContentPanelSectionDTO>();
                            cp.ContentPanelSections = cpsList;

                            cp.ContentPanelSections.Add(new ContentPanelSectionDTO
                            {
                                Name = "Details1",
                                PartialViewName = "~/Views/Sample/Details.cshtml",
                                Expanded = true
                            });

                            cp.ContentPanelSections.Add(new ContentPanelSectionDTO
                            {
                                Name = "Details2",
                                PartialViewName = "~/Views/Sample/Details.cshtml",
                                Expanded = true
                            });

                            cp.ContentPanelSections.Add(new ContentPanelSectionDTO
                            {
                                Name = "Details3",
                                PartialViewName = "~/Views/Sample/Details.cshtml",
                                Expanded = true
                            });
                        }
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
                cp = new ContentPanelDTO
                {
                    Name = "Sample Panel",
                    PartialViewName = "~/Views/Shared/_ContentTemplate.cshtml",
                    ContentPanelSections = new List<ContentPanelSectionDTO>()
                };

                var cpsList = new List<ContentPanelSectionDTO>();
                cp.ContentPanelSections = cpsList;

                cp.ContentPanelSections.Add(new ContentPanelSectionDTO
                {
                    Name = "Details1",
                    PartialViewName = "~/Views/Sample/Details.cshtml",
                    Expanded = true
                });

                cp.ContentPanelSections.Add(new ContentPanelSectionDTO
                {
                    Name = "Details2",
                    PartialViewName = "~/Views/Sample/Details.cshtml",
                    Expanded = true
                });

                cp.ContentPanelSections.Add(new ContentPanelSectionDTO
                {
                    Name = "Details3",
                    PartialViewName = "~/Views/Sample/Details.cshtml",
                    Expanded = true
                });
            }

            return cp;
        }

        [Obsolete]
        public static ContentPanelDTO GetApplicationPanel()
        {
            // TODO: Refactor this method to obtain data from Foresight Service

            var cp = new ContentPanelDTO
            {
                Name = "Content Template",
                PartialViewName = "~/Views/Shared/_ContentTemplate.cshtml", 
                ContentPanelSections = new List<ContentPanelSectionDTO>()
            };

            cp.ContentPanelSections.Add(new ContentPanelSectionDTO
            {
                Name = "View Application Information",
                PartialViewName = "~/Views/Application/Details.cshtml",
                Expanded = true
            });            

            cp.ContentPanelSections.Add(new ContentPanelSectionDTO
                {
                    Name = "View Locations",
                    PartialViewName = "~/Views/Application/ViewLocations.cshtml",
                    Expanded = true
                });

            cp.ContentPanelSections.Add(new ContentPanelSectionDTO
            {
                Name = "Manage Remarketing Campaign Files",
                PartialViewName = "~/Views/Application/RemarketingCampaigns.cshtml",
                Expanded = true
            });

            cp.ContentPanelSections.Add(new ContentPanelSectionDTO
            {
                Name = "Stats",
                PartialViewName = "~/Views/Application/Stats.cshtml",
                Expanded = true
            });

            return cp;
        }

        [Obsolete]
        public static ContentPanelDTO GetPackagePanel()
        {
            // TODO: Refactor this method to obtain data from Foresight Service

            var cp = new ContentPanelDTO
            {
                Name = "Content Template",
                PartialViewName = "~/Views/Shared/_ContentTemplate.cshtml",
                ContentPanelSections = new List<ContentPanelSectionDTO>()
            };

            cp.ContentPanelSections.Add(new ContentPanelSectionDTO
            {
                Name = "Package Information",
                PartialViewName = "~/Views/Package/Details.cshtml",
                Expanded = true
            });

            cp.ContentPanelSections.Add(new ContentPanelSectionDTO()
            {
                Name = "Containers",
                PartialViewName = "~/Views/Package/Containers.cshtml",
                Expanded =  true
            });
            
            return cp;
        }

        [Obsolete]
        public static ContentPanelDTO GetModelPanel()
        {
            // TODO: Refactor this method to obtain data from Foresight Service

            var cp = new ContentPanelDTO
            {
                Name = "Content Template",
                PartialViewName = "~/Views/Shared/_ContentTemplate.cshtml"
            };
            var cpsList = new List<ContentPanelSectionDTO>();
            cp.ContentPanelSections = cpsList;

            cp.ContentPanelSections.Add(new ContentPanelSectionDTO
            {
                Name = "Model Data",
                PartialViewName = "~/Views/Models/ModelData.cshtml",
                Expanded = true
            });            

            cp.ContentPanelSections.Add(new ContentPanelSectionDTO
            {
                Name = "Model Visualization",
                PartialViewName = "~/Views/Models/ModelVisualization.cshtml",
                Expanded = true
            });

            return cp;
        }

        public static void SetContentPanelSectionState(int contentPanelSectionID)
        {
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        var request = new SetContentPanelSectionStateRequest()
                        {
                            ContentPanelSectionID = contentPanelSectionID,
                            User = FormsAuthenticationWrapper.User
                        };

                        _certonaService.SetContentPanelSectionState(request);
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

            }
        }
    }
}