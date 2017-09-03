using System.Web.Mvc;
using System.IO;

namespace Resonance.Insight3.Web.Helpers
{
    public class HtmlHelpers
    {
    }

    public static class i18nHelper
    {
        public static string LocalResources(this WebViewPage page, string key)
        {
            return page.ViewContext.HttpContext.GetLocalResourceObject(page.VirtualPath, key) as string;
        }

        /// <summary>
        /// Helper method used to lookup localized strings in an InfoPanel resource file
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string InfoPanelResources(this WebViewPage page, string key)
        {
            string resource = GetSharedResource(page.VirtualPath);
            if (resource.EndsWith(".cshtml"))
                resource = resource.Replace(".cshtml", ".infopanel");

            return page.ViewContext.HttpContext.GetLocalResourceObject(resource, key) as string;
        }

        /// <summary>
        /// Helper method used to lookup localized strings in an Foresight specific area resource file
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string SharedResources(this WebViewPage page, string key)
        {
            string resource = GetSharedResource(page.VirtualPath);

            return page.ViewContext.HttpContext.GetLocalResourceObject(resource, key) as string;
        }

        #region Helper Methods

        private static string GetSharedResource(string resourceName)
        {
            // we have multiple views (View/Manage) needing access to same resource file, so do the mapping here
            // parse to get name of Partial View
            // i.e. ViewDetails.cshmtl or ManageDetails.cshtml

            string path = GetSharedResourcePath(resourceName);
            string partialViewName = GetSharedResourceName(resourceName);

            return string.Format("{0}{1}", path, partialViewName);
        }

        private static string GetSharedResourcePath(string resourceName)
        {
            string partialViewName = Path.GetFileName(resourceName);
            return resourceName.Replace(partialViewName, string.Empty);
        }

        private static string GetSharedResourceName(string resourceName)
        {
            string partialViewName = Path.GetFileName(resourceName);
            if (partialViewName.StartsWith("View"))
                partialViewName = partialViewName.Remove(0, 4);
            else if (partialViewName.StartsWith("Manage"))
                partialViewName = partialViewName.Remove(0, 6);
            return partialViewName;
        }

        #endregion
    }
}