using System.Collections.Generic;
using System.Data.SqlClient;
using CFRFException;

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
            builder.DataSource = "cfrfappdbserver.database.windows.net";
            builder.UserID = "vinicius";
            builder.Password = "CFRF@2018";
            builder.InitialCatalog = "CFRFApp_db";
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
                    using(SqlCommand command = new SqlCommand(query, connection) { CommandType = System.Data.CommandType.Text }) {
                        using(SqlDataReader reader = command.ExecuteReader()) {
                            while(reader.Read()) {
                                for(int i = 0; i < reader.VisibleFieldCount; i++) {
                                    queryResult.Add(reader.GetValue(i));
                                }
                            }
                        }
                    }
                } else {
                    throw new ResponseException("Erro", "Não foi possível se conectar ao banco de dados");
                }
                return queryResult;
            } catch {
                throw new ResponseException("Erro", "Não foi possível se conectar ao banco de dados");
            }
        }
        public void InsertByProc(string query, string code, byte[] image) {
            OpenConnection();
            SqlCommand command = new SqlCommand(query, connection) { CommandType = System.Data.CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Code", code);
            command.Parameters.AddWithValue("@image", image);
            try {
                command.ExecuteNonQuery();
            }catch {
                throw new ResponseException("Erro", "Não foi possível inserir imagem no banco de dados");
            }
        }
    }
}