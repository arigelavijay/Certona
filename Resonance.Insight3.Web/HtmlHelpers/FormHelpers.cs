using System.Collections.Generic;
using System.Web.Mvc;

namespace Resonance.Insight3.Web.HtmlHelpers
{
    public static class FormHelpers
    {
        public static List<SelectListItem> DropDownListInt(int startInt, int endInt)
        {
            var list = new List<SelectListItem>();

            for (var i = startInt; i < endInt; i++)
            {
                list.Add(new SelectListItem() {Text = i.ToString(), Value = i.ToString()});
            }

            return list;
        }
    }
}