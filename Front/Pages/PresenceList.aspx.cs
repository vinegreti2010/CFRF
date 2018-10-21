using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Front.Pages {
    public partial class PresenceList : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

        }
        protected void TryRunDiary_Click(object sender, EventArgs e) {
            if(ClassDropDownList.SelectedValue.Equals("")) {
                Msg.Text = "Todos os campos são obrigatórios";
                Msg.ForeColor = Color.Red;
                return;
            }

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("Report", "PL");
            parameters.Add("Strm", StrmDropDown.SelectedValue);
            parameters.Add("Class", ClassDropDownList.SelectedValue);

            Session["Parameters"] = parameters;

            string url = "ProcessingReport.aspx";
            string fullURL = "window.open('" + url + "', '_blank', 'height=500,width=800,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }
    }
}