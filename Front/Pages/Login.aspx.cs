using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EncodingSHA;
using Database;

namespace Front.Pages {
    public partial class Login : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected void TryLogin_Click(object sender, EventArgs e) {
            string username = usernameFld.Text;
            string password = passwordFld.Text;

            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) {
                invalidFld.Text = "**Usuário e senha são campos obrigatórios**";
                invalidFld.Visible = true;
                return;
            }

            if(username.Length <= 5 || password.Length <= 5) {
                invalidFld.Text = "**Usuário e senha devem possuir no mínimo 5 caracteres**";
                invalidFld.Visible = true;
                return;
            }

            string encryptedPass = Encrypt.EncryptStr(password + username.Substring(0, 2));

            DatabaseHandler database = Singleton<DatabaseHandler>.Instance();

            string query = "SELECT access FROM opr_defn WHERE user_id = '" + username + "' AND password_user = '" + encryptedPass + "';";

            if(database.ExecuteQuery(query).Count != 1) {
                invalidFld.Text = "**Usuário ou senha inválido**";
                invalidFld.Visible = true;
                return;
            }

            if(database.ExecuteQuery(query).Equals("N")) {
                invalidFld.Text = "**Acesso negado, favor contatar o suporte**";
                invalidFld.Visible = true;
                return;
            }

            invalidFld.Visible = false;
            Response.Redirect("http://www.google.com");
        }
    }
}