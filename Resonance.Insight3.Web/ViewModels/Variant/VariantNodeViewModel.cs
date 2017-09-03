using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.FormAuthentication;
using Resonance.Insight3.Web.Models;

namespace Resonance.Insight3.Web.ViewModels.Variant
{
    public class VariantNodeViewModel : ModelBase
    {
        public int ExperienceID { get; set; }
        public int VariantID { get; set; }
        
        [Required(ErrorMessage="Please select a Catalog")]
        public String CatalogID { get; set; }
        
        public Int32 Priority { get; set; }
        public Int32 NumberOfAssets { get; set; }
        public Int32 MethodID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Must be less than 100 characters")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [StringLength(255, ErrorMessage = "Must be less than 255 characters")]
        public string Description { get; set; }

        [StringLength(255, ErrorMessage = "Must be less than 255 characters")]
        public string ContainerTitle { get; set; }

#region "Drop Down Lists"

        public List<SelectListItem> PriorityList
        {
            get
            {
                var _lastPriority = 0;
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var request = new GetLastVariantPriorityByExperienceRequest()
                        {
                            User = user,
                            ExperienceID = this.ExperienceID
                        };
                        var response = _certonaService.GetLastVariantPriorityByExperience(request);

                        _lastPriority = response.LastPriority;
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

                var priorityList = new List<SelectListItem>();
                for (var i = 1; i <= (_lastPriority + 1); i++)
                {
                    priorityList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }
                return priorityList;
            }
        }

        public List<SelectListItem> ApplicationCatalogList
        {
            get
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var request = new GetApplicationCatalogsByExperienceRequest()
                            {
                                User = user,
                                ExperienceID = this.ExperienceID
                            };
                        var response = _certonaService.GetApplicationCatalogsByExperience(request);
                        var applicationCatalogList = new List<SelectListItem>();
                        applicationCatalogList.Add(new SelectListItem() { Selected = true, Text = "Select a Catalog", Value = "" });
                        applicationCatalogList.AddRange(response.Catalogs.Select(catalogId => new SelectListItem() { Text = catalogId.CatalogName, Value = catalogId.CatalogID }).ToList());
                        return applicationCatalogList;
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
        }

        public List<SelectListItem> NumberOfAssetsList
        {
            get
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var appDetailRequest = new GetSchemeNumberOfItemsByExperienceRequest { User = user, ExperienceID = this.ExperienceID };
                        var appDetailResponse = _certonaService.GetSchemeNumberOfItemsByExperience(appDetailRequest);
                        var numberOfItems = appDetailResponse.NumberOfItems;

                        var numberOfAssetsList = new List<SelectListItem>();
                        for (var i = 1; i <= numberOfItems; i++)
                        {
                            numberOfAssetsList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                        }
                        return numberOfAssetsList;
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
        }
#endregion
    }
}