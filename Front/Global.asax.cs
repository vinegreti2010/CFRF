using Database;
using System;

namespace Front {
    public class Global : System.Web.HttpApplication {
        DatabaseHandler database;
        protected void Application_Start(object sender, EventArgs e) {
            database = Singleton<DatabaseHandler>.Instance();
            database.OpenConnection();
        }

        protected void Dispose() => database.CloseConnection();
    }
}