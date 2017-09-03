namespace Resonance.Insight3.Web.ViewModels.Experience
{
    public class ExperienceStrategyDecisionPlan
    {
        public int StrategyID { get; set; }
        public int? NumberOfItems { get; set; }
        public int? MinItems { get; set; }
        public bool? BreakOnMinItems { get; set; }
        public string SlotInfo { get; set; }
    }
}