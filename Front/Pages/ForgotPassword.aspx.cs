using Database;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Front.Pages {
    public partial class ForgotPassword : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected void TryRecoveryPass_Click(object sender, EventArgs e) {
            string username = usernameFld.Text;
            if(string.IsNullOrEmpty(username) || string.IsNullOrWhiteSpace(username)) {
                msgFld.Text = "**Por favor, informe seu usuário**";
                msgFld.ForeColor = Color.Red;
                msgFld.Visible = true;
                return;
            }

            DatabaseHandler database = Singleton<DatabaseHandler>.Instance();
            string query = "SELECT email_addr FROM opr_defn WHERE user_id = '" + username + "';";

            List<Object[]> result = database.ExecuteQuery(query);

            if(database.ExecuteQuery(query).Count != 1) {
                msgFld.Text = "**Usuário não cadastrado**";
                msgFld.ForeColor = Color.Red;
                msgFld.Visible = true;
                return;
            }

            string email = (string)result[0][0];
            if(email == null) {
                msgFld.Text = "**Email não cadastrado**";
                msgFld.ForeColor = Color.Red;
                msgFld.Visible = true;
                return;
            }

            string[] splittedEmail = email.Split('@');

            if(splittedEmail.Length != 2) {
                msgFld.Text = "**Email cadastrado inválido**";
                msgFld.ForeColor = Color.Red;
                msgFld.Visible = true;
                return;
            }

            if(splittedEmail[0].Length <  3) {
                msgFld.Text = "**Email cadastrado inválido**";
                msgFld.ForeColor = Color.Red;
                msgFld.Visible = true;
                return;
            }

            msgFld.Text = "Email Enviado para \"" + splittedEmail[0].Substring(0, 2) + "***@" + splittedEmail[1] +"\"";
            msgFld.ForeColor = Color.Green;
            msgFld.Visible = true;
        }
    }
}