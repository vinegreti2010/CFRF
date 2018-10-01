using App_Start;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Threading;


namespace restServer {
    public class WebApiApplication : System.Web.HttpApplication {
        Thread facenetServerThread = null;
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            StartServices.StartDatabase();

            facenetServerThread = new Thread(StartServices.StartFacenetServer);
            facenetServerThread.Start();
            //StartServices.StartFacenetServer();
        }
        protected void Application_End() {
            facenetServerThread.Abort();
        }
    }
}
