using System;
using System.Collections.Generic;

namespace Resonance.Insight3.Web.ViewModels.Package
{
    public class PackageViewModel
    {
        public PackageDetails PackageDetails { get; set; }
        public List<PackageSchemesViewModel> PackageSchemes { get; set; }
    }

    public class PackageDetails
    {
        public Int32 PackageId { get; set; }
        public String PackageName { get; set; }
        public String Status { get; set; }
        public String PackageType { get; set; }
        public String Description { get; set; }
        public String SubType { get; set; }
        public string PageURL { get; set; }
    }

    public class PackageSchemesViewModel
    {
        public string Catalog { get; set; }
        public string CatalogImage { get; set; }
        public string ContainerID { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string NumberOfItems { get; set; }
        public string Status { get; set; }
    }
}