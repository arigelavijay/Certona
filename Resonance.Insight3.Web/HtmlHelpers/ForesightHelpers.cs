using System;
using System.Text;
using System.Web.Mvc;
using Resonance.Insight3.Web.ViewModels.Asset;
using Resonance.Insight3.Web.Helpers;

namespace Resonance.Insight3.Web.HtmlHelpers
{
    public static class ForesightHelpers
    {
        public static MvcHtmlString TriStateReadOnly(this HtmlHelper html, bool? value)
        {
            string img = value.HasValue ? (value.Value ? "checked_gray.png" : "unchecked_gray.png") : "intermediate_gray.png";
            return MvcHtmlString.Create(String.Format(@"<img src='/Images/{0}'/>", img));
        }

        public static MvcHtmlString TriStateRender(this HtmlHelper html, bool? value, string id)
        {
            /* render the follwoing
             <span id="exclCurrentAssets" style="cursor: default;">
                <input type="hidden" id="exclCurrentAssetsState" name="exclCurrentAssetsState" value="@state"/>
            </span>
            */
            return new MvcHtmlString(string.Empty);
            //return MvcHtmlString.Create().ToString());
        }
        
        public static MvcHtmlString ItemDetailSection(this HtmlHelper html, CatalogAssetMapping mappingDetails)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<div class='asset-additional-detail'>");
            sb.AppendFormat("<div style='padding-bottom: 5px'><span class='bold'>{0}</span></div>", mappingDetails.DisplayLabel);
            if (mappingDetails.DisplayValues.Count == 0)
            {
                sb.AppendLine("<div>&#160;</div>");
                sb.AppendLine("<div>&#160;</div>");
            }
            else
            {
                string displayVal = mappingDetails.DisplayValues[0];
                string tooltip = string.Empty;
                if (!String.IsNullOrWhiteSpace(mappingDetails.DisplayValues[0]))
                    tooltip = new MvcHtmlString(mappingDetails.DisplayValues[0]).ToHtmlString();
                sb.AppendFormat(
                    "<div class='asset-additional-detail-display' title='{1}'><span>{0}</span></div>",
                    mappingDetails.DisplayValues[0], tooltip);

                if (mappingDetails.DisplayValues.Count == 1)
                {
                    sb.AppendLine("<div>&nbsp</div>");
                }
                else
                {
                    string key = mappingDetails.DisplayLabel.Replace(" ", "_");
                    sb.AppendFormat(
                        "<div><span id='{1}' style='cursor: pointer' onclick='displayEnumeratedData(this);'><u>{0} more</u></span></div>",
                        mappingDetails.DisplayValues.Count - 1, key);
                }
            }
            sb.AppendLine("</div>");

            /*
            <div style="float: left; width: 33%; padding-bottom: 5px">
                <div style="padding-bottom: 5px"><span class='bold'>@Model.AdditionalDetails.Details[key].DisplayLabel</span></div>
                <div>@{ Write(Model.AdditionalDetails.Details[key].DisplayValues.Count > 0 ? Model.AdditionalDetails.Details[key].DisplayValues[0] : "&#160;"); }</div> 
            </div>
            */
            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString ShowRulesLink(this HtmlHelper html, bool showRules)
        {
            string dataMode = showRules ? "show" : "hide";
            string display = showRules ? "Hide Rules" : "Show Rules";            
            return MvcHtmlString.Create(String.Format(@"<span id='rulesShow' style='cursor:pointer' data-mode='{0}' onclick='toggleRules();'>{1}</span>", dataMode, display));
            
            /*
            <span id="rulesShow" style="cursor: pointer" data-mode="@{Write(@Model.ShowRules ? "show" : "hide");}" onclick="toggleRules();">@{ Write(@Model.ShowRules ? "Hide Rules" : "Show Rules");}</span> 
            */
        }

        public static MvcHtmlString ShowDecisionPlanLink(this HtmlHelper html, bool showDecisionPlan)
        {
            string dataMode = showDecisionPlan ? "show" : "hide";
            string display = showDecisionPlan ? "Hide Decision Plan" : "Show Decision Plan";
            return MvcHtmlString.Create(String.Format(@"<span id='decisionPlanShow' style='cursor:pointer' data-mode='{0}' onclick='toggleDecisionPlan();'>{1}</span>", dataMode, display));

            /*
            <span id="decisionPlanShow" style="cursor: pointer" data-mode="show" onclick="toggleDecisionPlan();">Hide Decision Plan</span>
            */
        }

        public static MvcHtmlString RequiredAsterik(this HtmlHelper html, WebViewPage page)
        {
            var tooltip = page.SharedResources("AsterikRequired");
            return
                MvcHtmlString.Create(String.Format("<span style=\"color: #ff0000;\" title=\"{0}\")\">*</span>", tooltip));
        }
    }
}