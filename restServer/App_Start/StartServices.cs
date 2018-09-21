using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App_Start {
    public class StartServices {
        public static void StartDatabase() {
            DatabaseHandler database = Singleton<DatabaseHandler>.Instance();
            database.OpenConnection();
        }
    }
}