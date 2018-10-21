using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Front.Pages {
    public partial class ClassRooms : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected void TryAccessRoom_Click(object sender, EventArgs e) {
            Session["FacilityId"] = DropDownRooms.SelectedValue;
            Session["loaded"] = false;
            Response.Redirect("../Pages/Rooms.aspx");
        }

        protected void TryCreateRoom_Click(object sender, EventArgs e) {
            Session["FacilityId"] = "-1";
            Session["loaded"] = false;
            Response.Redirect("../Pages/Rooms.aspx");
        }
    }
}