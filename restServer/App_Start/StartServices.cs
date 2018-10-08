using Database;
using System;
using System.Diagnostics;
using System.IO;

namespace App_Start {
    public class StartServices {
        DatabaseHandler database;
        public void StartDatabase() {
            database = Singleton<DatabaseHandler>.Instance();
            database.OpenConnection();
        }

        public void Dispose() {
            database.CloseConnection();
        }
    }
}