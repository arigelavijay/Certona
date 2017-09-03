using System.Web.Optimization;

namespace Resonance.Insight3.Web.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*",
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new StyleBundle("~/Content/login").Include(
                "~/Content/fonts/fonts-v.1.0.css",
                "~/Content/reset.css",
                "~/Content/grids.css",
                "~/Content/forms.css",
                "~/Content/common.v.1.0.css",
                "~/Content/login-styles.v.1.0.css"));

            bundles.Add(new StyleBundle("~/Content/home").Include(
                "~/Content/fonts/fonts-v.1.0.css",
                "~/Content/reset.css",
                "~/Content/grids.css",
                "~/Content/forms.css",
                "~/Content/common.v.1.0.css",
                "~/Content/Content.css",
                "~/Content/styles.v.1.1.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                "~/Content/themes/base/jquery.ui.core.css",
                "~/Content/themes/base/jquery.ui.resizable.css",
                "~/Content/themes/base/jquery.ui.selectable.css",
                "~/Content/themes/base/jquery.ui.accordion.css",
                "~/Content/themes/base/jquery.ui.autocomplete.css",
                "~/Content/themes/base/jquery.ui.button.css",
                "~/Content/themes/base/jquery.ui.dialog.css",
                "~/Content/themes/base/jquery.ui.slider.css",
                "~/Content/themes/base/jquery.ui.tabs.css",
                "~/Content/themes/base/jquery.ui.datepicker.css",
                "~/Content/themes/base/jquery.ui.progressbar.css",
                "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}