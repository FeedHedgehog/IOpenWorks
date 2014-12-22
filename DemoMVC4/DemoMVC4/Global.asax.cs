using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using UIShell.OSGi;
using UIShell.OSGi.Core.Service;
using System.IO;
using System.Web.Compilation;
using System.Reflection;
using UIShell.OSGi.Utility;
using UIShell.OSGi.Loader;
using System.Threading;
using UIShell.OSGi.MvcWebExtension;
using UIShell.iOpenWorks.Bootstrapper;

namespace DemoMVC4
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : BundleRuntimeMvcApplication
    {
        public override void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public override void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("DefaultPage", string.Empty, "~/Default.aspx");

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{plugin}/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected override void Application_Start(object sender, EventArgs e)
        {
            if (AutoUpdateCoreFiles)
            {
                new CoreFileUpdater().UpdateCoreFiles(CoreFileUpdateCheckType.Daily);
            }
            // Create a repository folder to store the downloaded plugins.
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "repository");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            base.Application_Start(sender, e);

            AreaRegistration.RegisterAllAreas();
        }

        /// <summary>
        /// 是否启用自动更新。
        /// </summary>
        private static bool AutoUpdateCoreFiles
        {
            get
            {
                string autoUpdateCoreFiles = System.Web.Configuration.WebConfigurationManager.AppSettings["AutoUpdateCoreFiles"];
                if (!string.IsNullOrEmpty(autoUpdateCoreFiles))
                {
                    try
                    {
                        return bool.Parse(autoUpdateCoreFiles);
                    }
                    catch { }
                }

                return false;
            }
        }
    }
}