using App_Start;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Threading;

namespace restServer {
    public class WebApiApplication : System.Web.HttpApplication {
        Thread facenetServerThread = null;
        StartServices Services;
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Services = new StartServices();
            Services.StartDatabase();

            facenetServerThread = new Thread(StartFacenet);
            facenetServerThread.Start();
        }
        protected void Application_End() {
            facenetServerThread.Abort();
            Services.Dispose();
        }

        private void StartFacenet() {
            Services.StartFacenetServer();
        }
    }
}
