using System.Web;
using System.Web.Optimization;

namespace PIS.AutoEnt.Portal.WebSite
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // base
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include("~/Content/themes/base/icon.css"));

            // jquery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/libs/jquery/jquery-{version}.js"));

            // extjs
            bundles.Add(new StyleBundle("~/Content/extjs/css").Include(
                "~/Scripts/libs/extjs4.2/resources/css/ext-all-neptune.css"));

            bundles.Add(new ScriptBundle("~/bundles/extjs").Include(
                "~/Scripts/libs/extjs4.2/ext-all-debug.js"));
            
            // app libs
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/app/common.js",
                "~/Scripts/app/pgctrl-ext4-ex.js",
                "~/Scripts/app/pginit-ext4.js",
                "~/Scripts/app/pgctrl-ext4.js",
                "~/Scripts/app/pgctrl-ext4-form.js",
                "~/Scripts/app/pgctrl-ext4-grid.js"
            ));
        }
    }
}