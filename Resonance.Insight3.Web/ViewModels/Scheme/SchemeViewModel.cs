namespace Resonance.Insight3.Web.ViewModels.Scheme
{
    public class SchemeDetails
    {
        public string ApplicationID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Scheme { get; set; }
        public string SchemeType { get; set; }
        public string DefaultCatalogID { get; set; }
        public string Status { get; set; }
        public short? RecMethod { get; set; }
        public string Callback { get; set; }
        public string CustomQueryString { get; set; }
        public int? NumberOfItems { get; set; }
        public string Icon_Filename { get; set; }
    }
}