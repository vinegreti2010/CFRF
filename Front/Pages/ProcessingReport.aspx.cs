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
                MemoryStream reportMemory = new MemoryStream();
                if(parameters["Report"].Equals("PL")) {
                    reportMemory = RunPL((string)parameters["Strm"], (string)parameters["Class"]);
                    reportFilename = "Lista de Presença";
                } else {
                    if(parameters["Report"].Equals("CD")) {
                        reportMemory = RunCD((string)parameters["Strm"], (string)parameters["Class"]);
                        reportFilename = "Diario de Classe";
                    }
                }

                if(reportFilename.Equals("")) {
                    Msg.Text = "Ocorreu um erro ao gerar o relatório, por favor contate o suporte";
                    Msg.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                                
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}.pdf", reportFilename));
                Response.BinaryWrite(reportMemory.ToArray());

            } catch(Exception ex) {
                Msg.Text = "Ocorreu um erro ao gerar o relatório, por favor contate o suporte";
                Msg.ForeColor = System.Drawing.Color.Red;
                return;
            }
        }

        private MemoryStream RunPL(string strm, string class_nbr) {
            Document document = new Document(PageSize.A4);
            document.SetMargins(40, 40, 40, 80);
            document.AddCreationDate();

            MemoryStream output = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, output);
            try {
                document.Open();

                string textData = "";
                Paragraph elements;

                elements = new Paragraph(textData, new Font(Font.NORMAL, 14));
                elements.Alignment = Element.ALIGN_CENTER;

                elements.Add("Lista de presença\n");
                document.Add(elements);

                elements = new Paragraph(textData, new Font(Font.NORMAL, 12));
                elements.Alignment = Element.ALIGN_LEFT;

                string query = "SELECT b.descr, a.descr from class_tbl a INNER JOIN term_tbl b ON b.strm = a.strm WHERE a.strm = '" + strm + "' AND a.class_nbr = '" + class_nbr + "'";
                List<Object[]> result = database.ExecuteQuery(query);

                elements.Add("\nPeríodo Letivo: " + result[0][0] + "\nAula: " + result[0][1] + "\n\n");
                document.Add(elements);

                query = "SELECT b.name_display from stdnt_enrl a INNER JOIN personal_data b ON b.student_id = a.student_id WHERE a.strm = '" + strm + "' AND a.class_nbr = '" + class_nbr + "'";
                result = database.ExecuteQuery(query);

                PdfPTable table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                Font tableFont = new Font(Font.NORMAL, 8);

                foreach(Object[] student in result) {
                    table.AddCell(new PdfPCell(new Phrase((string)student[0], tableFont)) { Border = Rectangle.NO_BORDER });
                    table.AddCell(new PdfPCell(new Phrase("______________________________________\n", tableFont)) { Border = Rectangle.NO_BORDER });
                }

                document.Add(table);
                
            } finally {
                document.Close();
            }

            return output;
        }

        private MemoryStream RunCD(string strm, string class_nbr) {
            Document document = new Document(PageSize.A4.Rotate());
            document.SetMargins(40, 40, 40, 80);
            document.AddCreationDate();

            MemoryStream output = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, output);
            try {
                document.Open();

                string textData = "";
                Paragraph elements;

                elements = new Paragraph(textData, new Font(Font.NORMAL, 14));
                elements.Alignment = Element.ALIGN_CENTER;

                elements.Add("Diário de Classe\n");
                document.Add(elements);

                elements = new Paragraph(textData, new Font(Font.NORMAL, 12));
                elements.Alignment = Element.ALIGN_LEFT;

                string query = "SELECT DISTINCT b.descr, a.descr, (select distinct count(1) from class_attendence where strm = a.strm and class_nbr = a.class_nbr group by student_id) as qty_dates, convert(varchar(10), convert(date, c.attend_dt)), convert(varchar(5), convert(time, c.start_time)) FROM class_tbl a INNER JOIN term_tbl b ON b.strm = a.strm INNER JOIN class_attendence c ON c.strm = a.strm AND c.class_nbr = a.class_nbr WHERE a.strm = '" + strm + "' AND a.class_nbr = '" + class_nbr + "' ORDER BY convert(varchar(10), convert(date, c.attend_dt)), convert(varchar(5), convert(time, c.start_time))";
                List<Object[]> result = database.ExecuteQuery(query);

                elements.Add("\nPeríodo Letivo: " + result[0][0] + "\nAula: " + result[0][1] + "\n\n");
                document.Add(elements);

                int qtyDates = (int)result[0][2];

                PdfPTable table = new PdfPTable(qtyDates + 1);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                Font tableFont = new Font(Font.NORMAL, 8);

                table.AddCell(new PdfPCell(new Phrase("Aluno", tableFont)));

                foreach(Object[] date in result) {
                    table.AddCell(new PdfPCell(new Phrase((string)date[3], tableFont)));
                }

                query = "SELECT b.name_display, a.presence, convert(varchar(10), convert(date, a.attend_dt)), convert(varchar(5), convert(time, a.start_time)) FROM class_attendence a INNER JOIN personal_data b ON b.student_id = a.student_id WHERE a.strm = '" + strm + "' AND a.class_nbr = '" + class_nbr + "' ORDER BY b.name_display, convert(varchar(10), convert(date, a.attend_dt)), convert(varchar(5), convert(time, a.start_time))";
                result = database.ExecuteQuery(query);

                for(int i = 0; i < result.Count; i += qtyDates) {
                    table.AddCell(new PdfPCell(new Phrase((string)result[i][0], tableFont)));
                    for(int j = i; j < (i + qtyDates); j++) {
                        table.AddCell(new PdfPCell(new Phrase((string)result[j][1], tableFont)));
                    }
                }
                document.Add(table);
            } finally {
                document.Close();
            }
            return output;
        }
    }
}