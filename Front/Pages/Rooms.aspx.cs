using Database;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Front.Pages {
    public partial class Rooms : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if(Session["FacilityId"] != null && Session["loaded"] != null) {
                if((bool)Session["loaded"] == false) {
                    fillValues(int.Parse((string)Session["FacilityId"]));
                    Session["loaded"] = true;
                }
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e) {
            if(!CheckFields()) {
                Msg.Text = "Todos os campos são obrigatórios";
                Msg.ForeColor = Color.Red;
                return;
            }

            try {
                ValidateLatitude(LatNE);
                ValidateLongitude(LongNE);

                ValidateLatitude(LatNW);
                ValidateLongitude(LongNW);

                ValidateLatitude(LatSE);
                ValidateLongitude(LongSE);

                ValidateLatitude(LatSW);
                ValidateLongitude(LongSW);

                DatabaseHandler database = Singleton<DatabaseHandler>.Instance();
                List<Tuple<string, object>> parameters = new List<Tuple<string, object>>();

                parameters.Add(new Tuple<string, object>("@FacilityId", int.Parse((string)Session["FacilityId"])));
                parameters.Add(new Tuple<string, object>("@Descr", Descr.Text));
                parameters.Add(new Tuple<string, object>("@LatNE", decimal.Parse(LatNE.Text)));
                parameters.Add(new Tuple<string, object>("@LongNE", decimal.Parse(LongNE.Text)));
                parameters.Add(new Tuple<string, object>("@LatNW", decimal.Parse(LatNW.Text)));
                parameters.Add(new Tuple<string, object>("@LongNW", decimal.Parse(LongNW.Text)));
                parameters.Add(new Tuple<string, object>("@LatSE", decimal.Parse(LatSE.Text)));
                parameters.Add(new Tuple<string, object>("@LongSE", decimal.Parse(LongSE.Text)));
                parameters.Add(new Tuple<string, object>("@LatSW", decimal.Parse(LatSW.Text)));
                parameters.Add(new Tuple<string, object>("@LongSW", decimal.Parse(LongSW.Text)));

                if(database.ExecuteProcedure("InsertCoordinates", parameters) <= 0) {
                    Msg.Text = "Erro ao Salvar";
                    Msg.ForeColor = Color.Red;
                    return;
                }
                Msg.Text = "Salvo com êxito";
                Msg.ForeColor = Color.Green;

                string query = "SELECT max(facility_id) FROM facility_tbl;";
                List<Object[]> result = database.ExecuteQuery(query);

                Session["FacilityId"] = ((int)result[0][0]).ToString();

                fillValues((int)result[0][0]);
            } catch { }
        }

        private void fillValues(int facilityId) {
            DatabaseHandler database = Singleton<DatabaseHandler>.Instance();
            string query = "SELECT * FROM facility_tbl WHERE facility_id = " + facilityId + ";";
            List<Object[]> result = database.ExecuteQuery(query);

            if(result.Count > 0) {
                RoomCode.Text = "Código: " + ((int)result[0][0]).ToString();
                Descr.Text = (string)result[0][1];
                LatNE.Text = ((decimal)result[0][2]).ToString();
                LongNE.Text = ((decimal)result[0][3]).ToString();
                LatNW.Text = ((decimal)result[0][4]).ToString();
                LongNW.Text = ((decimal)result[0][5]).ToString();
                LatSE.Text = ((decimal)result[0][6]).ToString();
                LongSE.Text = ((decimal)result[0][7]).ToString();
                LatSW.Text = ((decimal)result[0][8]).ToString();
                LongSW.Text = ((decimal)result[0][9]).ToString();
            } else {
                RoomCode.Text = "Código: ";
            }
        }

        private bool CheckFields() {
            if(string.IsNullOrEmpty(Descr.Text) || string.IsNullOrWhiteSpace(Descr.Text))
                return false;

            if(string.IsNullOrEmpty(LatNE.Text) || string.IsNullOrWhiteSpace(LatNE.Text))
                return false;

            if(string.IsNullOrEmpty(LongNE.Text) || string.IsNullOrWhiteSpace(LongNE.Text))
                return false;

            if(string.IsNullOrEmpty(LatNW.Text) || string.IsNullOrWhiteSpace(LatNW.Text))
                return false;

            if(string.IsNullOrEmpty(LongNW.Text) || string.IsNullOrWhiteSpace(LongNW.Text))
                return false;

            if(string.IsNullOrEmpty(LatSE.Text) || string.IsNullOrWhiteSpace(LatSE.Text))
                return false;

            if(string.IsNullOrEmpty(LongSE.Text) || string.IsNullOrWhiteSpace(LongSE.Text))
                return false;

            if(string.IsNullOrEmpty(LatSW.Text) || string.IsNullOrWhiteSpace(LatSW.Text))
                return false;

            if(string.IsNullOrEmpty(LongSW.Text) || string.IsNullOrWhiteSpace(LongSW.Text))
                return false;

            return true;
        }

        private void ValidateLatitude(TextBox textBox) {
            if(decimal.Parse(textBox.Text) < -90 || decimal.Parse(textBox.Text) > 90) {
                Msg.Text = "Valor deve ser decimal com valor entre -90 e 90";
                Msg.ForeColor = Color.Red;
                textBox.Focus();
                throw new Exception();
            }
        }

        private void ValidateLongitude(TextBox textBox) {
            if(decimal.Parse(textBox.Text) < -180 || decimal.Parse(textBox.Text) > 180) {
                Msg.Text = "Valor deve ser decimal com valor entre -180 e 180";
                Msg.ForeColor = Color.Red;
                textBox.Focus();
                throw new Exception();
            }
        }
    }
}