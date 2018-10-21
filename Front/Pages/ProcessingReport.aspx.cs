using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Database;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Front.Pages {
    public partial class ProcessingReport : System.Web.UI.Page {
        DatabaseHandler database;
        protected void Page_Load(object sender, EventArgs e) {
            if(Session["logged"] == null)
                Response.Redirect("~/Pages/Login.aspx");

            if(!(bool)Session["logged"])
                Response.Redirect("~/Pages/Login.aspx");


            Dictionary<string, object> parameters = (Dictionary<string, object>)Session["Parameters"];
            if(parameters == null) {
                Msg.Text = "Parametros inválidos";
                Msg.ForeColor = System.Drawing.Color.Red;
                return;
            }
            if(parameters.Count == 0) {
                Msg.Text = "Parametros inválidos";
                Msg.ForeColor = System.Drawing.Color.Red;
                return;
            }
            try {
                database = Singleton<DatabaseHandler>.Instance();
                string reportFilename = "";

                if(parameters["Report"].Equals("PL"))
                    reportFilename = RunPL((string)parameters["Strm"], (string)parameters["Class"]);
                if(parameters["Report"].Equals("CD"))
                    reportFilename = RunCD((string)parameters["Strm"], (string)parameters["Class"]);

                if(reportFilename.Equals("")) {
                    Msg.Text = "Ocorreu um erro ao gerar o relatório, por favor contate o suporte";
                    Msg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                Response.Redirect("../Reports/" + reportFilename);

            } catch(Exception ex) {
                Msg.Text = "Ocorreu um erro ao gerar o relatório, por favor contate o suporte";
                Msg.ForeColor = System.Drawing.Color.Red;
                return;
            }
        }

        private string RunPL(string strm, string class_nbr) {
            string query = "SELECT b.descr, a.descr from class_tbl a INNER JOIN term_tbl b ON b.strm = a.strm WHERE a.strm = '" + strm + "' AND a.class_nbr = '" + class_nbr + "'";
            List<Object[]> result = database.ExecuteQuery(query);

            Document document = new Document(PageSize.A4);
            document.SetMargins(40, 40, 40, 80);//estibulando o espaçamento das margens que queremos
            document.AddCreationDate();//adicionando as configuracoes

            string filepath = "../Reports/", filename = "PresenceList"+strm+ DateTime.Now.TimeOfDay.Ticks+class_nbr+".pdf";
            string fullName = @filepath + filename;

            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(fullName, FileMode.Create));

            document.Open();

            string textData = "";
            Paragraph elements;

            elements = new Paragraph(textData, new Font(Font.NORMAL, 18));
            elements.Alignment = Element.ALIGN_CENTER;

            elements.Add("Lista de presença\n");
            document.Add(elements);

            elements = new Paragraph(textData, new Font(Font.NORMAL, 14));
            elements.Alignment = Element.ALIGN_LEFT;

            elements.Add("\nPeríodo Letivo: " + result[0][0] + "\nAula: " + result[0][1]);
            document.Add(elements);

            query = "SELECT b.name_display from stdnt_enrl a INNER JOIN personal_data b ON b.student_id = a.student_id WHERE a.strm = '" + strm + "' AND a.class_nbr = '" + class_nbr + "'";
            result = database.ExecuteQuery(query);

            elements = new Paragraph(textData, new Font(Font.NORMAL, 12));
            elements.Alignment = Element.ALIGN_LEFT;
            elements.Add("\n\n");

            foreach(Object[] student in result) {
                elements.Add(student[0] + "\t\t_____________________________________");
            }
            
            document.Add(elements);

            document.Close();

            return fullName;
        }

        private string RunCD(string strm, string class_nbr) {


            return "";
        }
    }
}