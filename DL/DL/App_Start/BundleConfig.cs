using System.Web;
using System.Web.Optimization;

namespace DL.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.validate*"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/thirdpartyjs").Include(
                        "~/Scripts/jquery-ui-1.11.4.min.js",
                        "~/Scripts/flipclock/flipclock.min.js",
                        "~/Scripts/jquerycookie/jquery.cookie.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/moment.min.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/lumino.glyphs.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/styles.css"));

            bundles.Add(new StyleBundle("~/Content/thirdpartycss").Include(
                      "~/Content/jquery-ui-1.11.4.custom/jquery-ui.min.css",
                      "~/Content/flipclock/flipclock.css"));
        }
    }
}
