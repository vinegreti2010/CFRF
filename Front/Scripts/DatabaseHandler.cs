using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Configuration;

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
            builder.DataSource = WebConfigurationManager.AppSettings["DBServer"];
            builder.UserID = WebConfigurationManager.AppSettings["User"];
            builder.Password = WebConfigurationManager.AppSettings["Password"];
            builder.InitialCatalog = WebConfigurationManager.AppSettings["Database"];
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

        //public List<object> ExecuteQuery(string query) {
        //    try {
        //        List<object> queryResult = new List<object>();
        //        if(this.connection.State == System.Data.ConnectionState.Open) {
        //            using(SqlCommand command = new SqlCommand(query, connection) { CommandType = System.Data.CommandType.Text }) {
        //                using(SqlDataReader reader = command.ExecuteReader()) {
        //                    while(reader.Read()) {
        //                        for(int i = 0; i < reader.VisibleFieldCount; i++) {
        //                            queryResult.Add(reader.GetValue(i));
        //                        }
        //                    }
        //                }
        //            }
        //        } else {
        //            throw new Exception("Não foi possível se conectar ao banco de dados");
        //        }
        //        return queryResult;
        //    } catch(Exception e) {
        //        throw new Exception("Não foi possível se conectar ao banco de dados");
        //    }
        //}
        public List<Object[]> ExecuteQuery(string query) {
            try {
                List<Object[]> queryResult = new List<Object[]>();
                Object[] line = null;
                if(this.connection.State == System.Data.ConnectionState.Open) {
                    using(SqlCommand command = new SqlCommand(query, connection) { CommandType = System.Data.CommandType.Text }) {
                        using(SqlDataReader reader = command.ExecuteReader()) {
                            while(reader.Read()) {
                                line = new Object[reader.FieldCount];
                                    reader.GetValues(line);
                                queryResult.Add(line);
                            }
                        }
                    }
                } else {
                    throw new Exception("Não foi possível se conectar ao banco de dados");
                }
                return queryResult;
            } catch(Exception e) {
                throw new Exception("Não foi possível se conectar ao banco de dados");
            }
        }
        public int ExecuteProcedure(string procedure, List<Tuple<string, object>> parameters) {
            OpenConnection();
            SqlCommand command = new SqlCommand(procedure, connection) { CommandType = System.Data.CommandType.StoredProcedure };
            foreach(Tuple<string, object> param in parameters) {
                command.Parameters.AddWithValue(param.Item1, param.Item2);
            }
            try {
                return command.ExecuteNonQuery();
            }catch {
                throw new Exception("Não foi possível inserir imagem no banco de dados");
            }
        }
    }
}