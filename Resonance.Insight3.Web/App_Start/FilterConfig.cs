using System.Web.Mvc;

namespace Resonance.Insight3.Web.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ElmahHandleErrorAttribute());
            //filters.Add(new HandleErrorAttribute());
        }
    }
}