using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Kendo.Mvc.UI;

namespace Resonance.Insight3.Web.HtmlHelpers
{
    public static class HtmlExtensions
    {
        public class ImageSelectListItem : SelectListItem
        {
            public string ImageFileName { get; set; }
        }

        public static IHtmlString HtmlEncode(this HtmlHelper html, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new HtmlString("&#160;");
            }

            return new HtmlString(html.Encode(value));
        }

        public static string UrlPrependHttp(string url)
        {
            string newUrl = string.Empty;
            if (!string.IsNullOrEmpty(url))
            {
                string Prefix = "http://";
                string SecurePrefix = "https://";
                newUrl = url;
                if ((url.Substring(0, 7) != Prefix) && (url.Substring(0, 8) != SecurePrefix))
                {
                    newUrl = string.Format("{0}{1}", Prefix, url);
                }
            }
            return newUrl;
        }

        public static MvcHtmlString TextBoxForWithDefault<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                             Expression<Func<TModel, TProperty>>
                                                                                 expression, object originalValue, int width)
        {
            return htmlHelper.TextBoxFor(expression, new {data_original_value = originalValue, style = "width:" + width + "px;"});
        }

        public static MvcHtmlString TextAreaForWithDefault<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                             Expression<Func<TModel, TProperty>>
                                                                                 expression, object originalValue, int rows, int cols)
        {
            return htmlHelper.TextAreaFor(expression, rows, cols, new { data_original_value = originalValue });
        }

        public static MvcHtmlString CheckBoxForWithDefault<TModel>(this HtmlHelper<TModel> htmlHelper,
                                                                             Expression<Func<TModel, bool>>
                                                                                 expression, object originalValue)
        {
            return htmlHelper.CheckBoxFor(expression, new { data_original_value = originalValue });
        }

        public static MvcHtmlString PasswordForWithDefault<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                              Expression<Func<TModel, TProperty>>
                                                                                  expression, object originalValue)
        {
            return htmlHelper.PasswordFor(expression, new { data_original_value = originalValue });
        }

        public static MvcHtmlString RadioButtonForWithDefault<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                      Expression<Func<TModel, TProperty>> expression,
                                                                      object value, object originalValue)
        {
            return htmlHelper.RadioButtonFor(expression, value, new {data_original_value = originalValue});
        }

        public static MvcHtmlString DropDownListForWithDefault<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                                  Expression<Func<TModel, TProperty>>
                                                                                      expression,
                                                                                  IEnumerable<SelectListItem> selectList,
                                                                                  object originalValue)
        {
            return htmlHelper.DropDownListFor(expression, selectList, new {data_original_value = originalValue});
        }

        public static MvcHtmlString DropDownListForWithDefault<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                                  Expression<Func<TModel, TProperty>>
                                                                                      expression,
                                                                                  IEnumerable<SelectListItem> selectList,
                                                                                  object originalValue,int width)
        {
            return htmlHelper.DropDownListFor(expression, selectList, new { data_original_value = originalValue, style = string.Format("width:{0}px;", width.ToString()) });
        }

        public static MvcHtmlString ListBoxForWithDefault<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                  Expression<Func<TModel, TProperty>> expression,
                                                                  IEnumerable<SelectListItem> selectList,
                                                                  object originalValue)
        {
            return htmlHelper.ListBoxFor(expression, selectList, new { data_original_value = originalValue });
        }
    }
}