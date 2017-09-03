using System;
using System.Collections.Generic;
using System.ServiceModel;
using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.FormAuthentication;
using Resonance.Insight3.Web.HtmlHelpers;
using Resonance.Insight3.Web.ViewModels.Package;

namespace Resonance.Insight3.Web.Models
{
    public class PackageModel : ModelBase
    {
        public static PackageDetails GetPackageDetails(int packageID)
        {
            PackageDetails details = null;
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var packageDetailsRequest = new GetPackageDetailsRequest() { User = user, PackageID = packageID };
                        var packageDetailsResponse = _certonaService.GetPackageDetails(packageDetailsRequest);

                        if (packageDetailsResponse.Success && packageDetailsResponse.PackageDetails != null)
                        {
                            details = new PackageDetails()
                            {
                                PackageId = packageID,
                                Description = packageDetailsResponse.PackageDetails.Description,
                                PackageName = packageDetailsResponse.PackageDetails.Name,
                                PackageType = packageDetailsResponse.PackageDetails.Type,
                                SubType = packageDetailsResponse.PackageDetails.SubType,
                                PageURL = HtmlExtensions.UrlPrependHttp(packageDetailsResponse.PackageDetails.PageURL),
                                Status = null           // MISSING from service response
                            };
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

            }
            return details;
        }

        public static List<PackageSchemesViewModel> GetPackageSchemes(int packageID)
        {
            var packageSchemes = new List<PackageSchemesViewModel>();

            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var packageSchemesRequest = new GetPackageSchemesRequest() { User = user, PackageID = packageID };
                        var packageSchemesResponse = _certonaService.GetPackageSchemes(packageSchemesRequest);
                        if (packageSchemesResponse.Success
                            && packageSchemesResponse.PackageSchemes != null
                            && packageSchemesResponse.PackageSchemes.Count > 0)
                        {
                            foreach (PackageSchemesDTO package in packageSchemesResponse.PackageSchemes)
                            {
                                packageSchemes.Add(new PackageSchemesViewModel
                                {
                                    Catalog = package.Catalog,
                                    CatalogImage = package.CatalogImage,
                                    ContainerID = package.ContainerID,
                                    Description = package.Description,
                                    Name = package.Name,
                                    NumberOfItems = package.NumberOfItems,
                                    Status = package.Status
                                });
                            }
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

            }

            return packageSchemes;
        }

        [Obsolete]
        public static PackageViewModel GetPackageViewModel(NodeType nodeType, int packageID)
        {
            // Load up the top level Package view model
            var vm = new PackageViewModel();
            
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;

                        var packageDetailsRequest = new GetPackageDetailsRequest() { User = user, PackageID = packageID };
                        var packageDetailsResponse = _certonaService.GetPackageDetails(packageDetailsRequest);

                        if (packageDetailsResponse.Success && packageDetailsResponse.PackageDetails != null)
                        {
                            var dto = packageDetailsResponse.PackageDetails;
                            var ap = new PackageDetails()
                            {
                                PackageId = packageID,
                                Description = dto.Description,
                                PackageName = dto.Name,
                                PackageType = dto.Type,
                                SubType = dto.SubType,
                                PageURL = dto.PageURL,
                                Status = null           // MISSING from service response
                            };
                            vm.PackageDetails = ap;
                        }

                        var packageSchemesRequest = new GetPackageSchemesRequest() { User = user, PackageID = packageID };
                        var packageSchemesResponse = _certonaService.GetPackageSchemes(packageSchemesRequest);

                        if (packageSchemesResponse.Success
                            && packageSchemesResponse.PackageSchemes != null
                            && packageSchemesResponse.PackageSchemes.Count > 0)
                        {
                            vm.PackageSchemes = new List<PackageSchemesViewModel>();

                            foreach (PackageSchemesDTO package in packageSchemesResponse.PackageSchemes)
                            {
                                vm.PackageSchemes.Add(new PackageSchemesViewModel
                                {
                                    Catalog = package.Catalog,
                                    CatalogImage = package.CatalogImage,
                                    ContainerID = package.ContainerID,
                                    Description = package.Description,
                                    Name = package.Name,
                                    NumberOfItems = package.NumberOfItems,
                                    Status = package.Status
                                });
                            }
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

            }

            return vm;
        }
    }
}