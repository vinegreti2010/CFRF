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
            if(Session["logged"] != null)
                if((bool)Session["logged"])
                    Response.Redirect("~/Pages/Home.aspx");

        }

        protected void TryLogin_Click(object sender, EventArgs e) {
            string username = usernameFld.Text;
            string password = passwordFld.Text;

            Session["logged"] = false;

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

            List<Object[]> result = database.ExecuteQuery(query);

            if(result.Count != 1) {
                invalidFld.Text = "**Usuário ou senha inválido**";
                invalidFld.Visible = true;
                return;
            }

            if(result[0][0].Equals("N")) {
                invalidFld.Text = "**Acesso negado, favor contatar o suporte**";
                invalidFld.Visible = true;
                return;
            }

            Session["logged"] = true;

            invalidFld.Visible = false;
            Response.Redirect("~/Pages/Home.aspx");
        }
    }
}