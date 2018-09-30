using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using Informations;
using Newtonsoft.Json;

namespace Database {
    public sealed class Singleton<T> where T : class, new() {
        private static T instance;
        private static readonly object padlock = new object();

        public static T Instance() {
            lock(padlock) {
                if(instance == null)
                    instance = new T();
            }
            return instance;
        }
    }

    public class DatabaseHandler {

        private SqlConnectionStringBuilder builder;
        SqlConnection connection;

        public DatabaseHandler() {
            builder = new SqlConnectionStringBuilder();
            builder.DataSource = "cfrfdbserver.database.windows.net";
            builder.UserID = "vinicius";
            builder.Password = "CFRF@2018";
            builder.InitialCatalog = "CFRFDataBase";
        }

        public void OpenConnection() {
            if(this.connection == null)
                this.connection = new SqlConnection(builder.ConnectionString);

            if(this.connection.State != System.Data.ConnectionState.Open)
                this.connection.Open();
        }

        public void CloseConnection() {
            if(this.connection.State != System.Data.ConnectionState.Closed)
                this.connection.Close();
        }

        public List<object> ExecuteQuery(string query) {
            try {
                List<object> queryResult = new List<object>();
                if(this.connection.State == System.Data.ConnectionState.Open) { 
                    using(SqlCommand command = new SqlCommand(query, connection)) {
                        using(SqlDataReader reader = command.ExecuteReader()) {
                            while(reader.Read()) {
                                for(int i = 0; i < reader.VisibleFieldCount; i++) {
                                    queryResult.Add(reader.GetString(i));
                                }
                            }
                        }
                    }
                } else {
                    ResponseInfo response = new ResponseInfo {
                        code = "7",
                        header = "Erro",
                        message = "Não foi possível se conectar ao banco de dados"
                    };
                    throw new Exception(JsonConvert.SerializeObject(response));
                }
                return queryResult;
            } catch {
                ResponseInfo response = new ResponseInfo {
                    code = "7",
                    header = "Erro",
                    message = "Não foi possível se conectar ao banco de dados"
                };
                throw new Exception(JsonConvert.SerializeObject(response));
            }
        }
    }
}