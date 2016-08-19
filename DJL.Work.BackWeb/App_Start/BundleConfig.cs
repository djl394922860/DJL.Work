using System.Web;
using System.Web.Optimization;

namespace DJL.Work.BackWeb
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //using
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            //自定义
            bundles.Add(new StyleBundle("~/Content/mycss").Include(
                "~/Content/EasyUI/1.4.4/themes/default/easyui.css",
                "~/Content/Site.css",
                "~/Content/EasyUI/1.4.4/themes/icon.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/easyui").Include(
                "~/Content/EasyUI/1.4.4/jquery.easyui.min.js",
                "~/Content/EasyUI/1.4.4/locale/easyui-lang-zh_CN.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/validata").Include(
                "~/Content/jqueryvalidation/jquery.validate.min.js",
                "~/Content/jqueryvalidation/messages_zh.min.js"
                ));
        }
    }
}
