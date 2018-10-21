using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Front.Pages {
    public partial class Layout : System.Web.UI.MasterPage {
        protected void Page_Load(object sender, EventArgs e) {
            if(Session["logged"] == null)
                Response.Redirect("~/Pages/Login.aspx");

            if(!(bool)Session["logged"])
                Response.Redirect("~/Pages/Login.aspx");
        }

        protected void TryLogout_Click(object sender, EventArgs e) {
            Session["logged"] = false;
            Session.Clear();

            Response.Redirect("~/Pages/Login.aspx");
        }
    }
}