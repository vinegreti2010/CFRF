using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using Informations;
using Newtonsoft.Json;

namespace cSharp {
    public sealed class DataBaseHandler {

        private static DataBaseHandler instance = null;
        private static readonly object padlock = new object();
        private SqlConnectionStringBuilder builder;

        private DataBaseHandler() {
            builder = new SqlConnectionStringBuilder();
            builder.DataSource = "cfrfdbserver.database.windows.net";
            builder.UserID = "vinicius";
            builder.Password = "Vine!@#1015Souto";
            builder.InitialCatalog = "CFRFDataBase";
        }

        public static DataBaseHandler GetInstance {
            get {
                lock(padlock) {
                    if(instance == null) {
                        instance = new DataBaseHandler();
                    }
                    return instance;
                }
            }
        }

        public List<object> ExecuteQuery(string query) {
            try {
                List<object> returnValues = new List<object>();
                using(SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();
                    using(SqlCommand command = new SqlCommand(query, connection)) {
                        using(SqlDataReader reader = command.ExecuteReader()) {
                            while(reader.Read()) {
                                for(int i = 0; i < reader.VisibleFieldCount; i++) {
                                    returnValues.Add(reader.GetString(i));
                                }
                            }
                        }
                    }
                }
                return returnValues;

                
            } catch {
                ResponseInfo response = new ResponseInfo {
                    code = "7",
                    header = "Erro",
                    message = "Não foi possível conectar ao banco de dados"
                };
                throw new Exception(JsonConvert.SerializeObject(response));
            }
        }
    }
}