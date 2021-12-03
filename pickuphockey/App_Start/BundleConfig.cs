using System.Web.Optimization;

namespace pickuphockey
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
						"~/Scripts/jquery.datetimepicker.full.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/themes/base/*.css",
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

			bundles.Add(new ScriptBundle("~/bundles/moment").Include(
					  "~/Scripts/moment.js"));

			// Added main
			bundles.Add(new ScriptBundle("~/bundles/main").Include(
                      "~/Scripts/main.js"));

            bundles.Add(new ScriptBundle("~/bundles/onesignal").Include(
                      "~/Scripts/OneSignalSDK.js"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
