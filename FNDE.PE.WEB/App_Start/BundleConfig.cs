using System.Web;
using System.Web.Optimization;

namespace FDNE.PE.WEB.PORTAL
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        //"~/Content/lib/jquery/jquery-{version}.js"));
                        "~/Content/lib/jquery/jquery.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Content/lib/jquery-validate/jquery.validate.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        //"~/Content/lib/modernizr/modernizr*"));
                        "~/Content/lib/modernizr/modernizr.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/lib/bootstrap/dist/js/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/lib/bootstrap/dist/css/bootstrap.css",
                      "~/Content/css/Site.css"));
        }
    }
}
