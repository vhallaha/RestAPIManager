using System.Web.Optimization;

namespace UI.Presentation
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/utilities").Include(
                        "~/content/lib/jquery/jquery-3.2.1.min.js",
                        "~/content/lib/bootstrap/js/bootstrap.js",
                        "~/content/lib/skylo/skylo.js",
                        "~/content/lib/scrollbar/js/perfect-scrollbar.jquery.min.js",
                        "~/content/lib/toastr/toastr.min.js",
                        "~/content/js/ui.utilities.js",
                        "~/content/js/ui.form.js", 
                        "~/content/js/ui.eventHandlers.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/content/lib/bootstrap/css/bootstrap.css",
                        "~/content/lib/toastr/toastr.min.css",
                        "~/content/lib/scrollbar/css/perfect-scrollbar.min.css",
                        "~/content/lib/skylo/skylo.css"
                        ));

            bundles.Add(new LessBundle("~/Content/less").Include(
                    "~/content/less/ui.global.less",
                    "~/content/less/ui.navi.less",
                    "~/content/less/ui.site.less"
                ));
        }
    }
}
