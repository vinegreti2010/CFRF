using Database;
using System;
using System.Web;

namespace Front {
    public class Global : System.Web.HttpApplication {
        DatabaseHandler database;
        protected void Application_Start(object sender, EventArgs e) {
            database = Singleton<DatabaseHandler>.Instance();
            database.OpenConnection();
        }

        //protected void Application_Error(object sender, EventArgs e) {
        //    if(HttpContext.Current == null)
        //        return;


        //    HttpContext context = HttpContext.Current;


        //    Exception exception = context.Server.GetLastError();


        //    string errorInfo = "Página em manutenção, por favor, tente mais tarde";

        //    context.Response.Write(errorInfo);
        //    context.Server.ClearError();
        //}

        protected void Dispose() => database.CloseConnection();
    }
}