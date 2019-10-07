using ActivePDFToolkitAzureWebApp.Net472.App_Start;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ActivePDFToolkitAzureWebApp.Net472
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Delete all PDF files on start up
            try
            {
                DirectoryInfo dInfo = new DirectoryInfo(Server.MapPath("~"));
                FileInfo[] pdfFiles = dInfo.GetFiles("*.pdf").Where(p => p.Extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase)).ToArray();
                foreach (FileInfo fInfo in pdfFiles)
                {
                    fInfo.Attributes = FileAttributes.Normal;
                    File.Delete(fInfo.FullName);
                }
            }
            catch(Exception e)
            {
            }
            
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
