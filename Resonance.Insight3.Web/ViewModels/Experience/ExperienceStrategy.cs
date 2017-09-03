namespace Resonance.Insight3.Web.ViewModels.Experience
{
    public class ExperienceStrategy
    {
        public int StrategyID { get; set; }
        public string Name { get; set; }
        public double? Impressions { get; set; }
        public string Catalog { get; set; }
        public string CatalogImage { get; set; }
        public string PersonalizationType { get; set; }
        public string Status { get; set; }
        public int? Priority { get; set; }
        public string Rules { get; set; }
        public int? NumberOfItems { get; set; }
        public int? MinItemsReturned { get; set; }
        public bool? BreakOnMinItemsReturned { get; set; }
    }
}