using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using CFRFException;
using Database;
using Facenet;
using Informations;
using Ionic.Crc;
using Ionic.Zip;

class ResultGrid {
    public string Status { get; set; }
    public string Log { get; set; }
}

namespace Front.Pages {
    public partial class UploadImages : System.Web.UI.Page {
        DatabaseHandler database;
        protected void Page_Load(object sender, EventArgs e) {
            database = Singleton<DatabaseHandler>.Instance();
        }

        protected void TryUpload_Click(object sender, EventArgs e) {
            ResultGrd.DataSource = null;
            ResultGrd.DataBind();
            Msg.Text = "";
            if(!FileFld.HasFile) {
                Msg.Text = "É obrigatório inserir um arquivo";
                Msg.ForeColor = Color.Red;
                return;
            }

            if(!FileFld.FileName.EndsWith(".zip")) {
                Msg.Text = "Apenas arquivos .zip são aceitos";
                Msg.ForeColor = Color.Red;
                return;
            }

            List<ResultGrid> results = new List<ResultGrid>();
            string query = "";
            string[] filename;
            List<Object[]> queryResult;

            using(ZipFile zip = ZipFile.Read(FileFld.PostedFile.InputStream)) {
                results.Add(new ResultGrid() { Status = "Iniciando", Log = "Iniciando leitura de " + zip.Count + " imagens" });
                ResultGrd.DataSource = results;
                ResultGrd.DataBind();

                foreach(ZipEntry entry in zip) {
                    filename = entry.FileName.Split('.');

                    if(!filename[1].Equals("jpg") && !filename[1].Equals("jpeg") && !filename[1].Equals("png")) {
                        results.Add(new ResultGrid() { Status = "Erro", Log = "O arquivo " + entry.FileName + " não corresponde a uma imagem.\nApenas arquivos dos tipos .jpg, .jpeg e .png são aceitos" });
                        ResultGrd.DataSource = results;
                        ResultGrd.DataBind();
                        continue;
                    }

                    CrcCalculatorStream reader = entry.OpenReader();
                    MemoryStream memoryStream = new MemoryStream();
                    reader.CopyTo(memoryStream);
                    byte[] imgByte = memoryStream.ToArray();
                    
                    query = "SELECT name_display FROM personal_data A WHERE A.student_id = '" + filename[0] + "';";
                    queryResult = database.ExecuteQuery(query);

                    if(queryResult.Count == 0) {
                        results.Add(new ResultGrid() { Status = "Erro", Log = "Aluno " + filename[0] + " não cadastrado, realize a integração com o sistema acadêmico." });
                        ResultGrd.DataSource = results;
                        ResultGrd.DataBind();
                        continue;
                    }

                    List<string> result = ApplyImage(imgByte, filename[0], (string)queryResult[0][0]);
                    results.Add(new ResultGrid() { Status = result[0], Log = result[1] });
                    ResultGrd.DataSource = results;
                    ResultGrd.DataBind();
                }
            }
            results.Add(new ResultGrid() { Status = "Encerrando", Log = "Processamento concluído" });
            ResultGrd.DataSource = results;
            ResultGrd.DataBind();
        }

        private List<string> ApplyImage(byte[] imgByte, string code, string name) {
            try {
                FacenetHandler facenet = new FacenetHandler();
                WebRequest request = facenet.SendMessage("hasFace", "POST", new ImagetoDb() { img = Convert.ToBase64String(imgByte) });
                ImagetoDbResponse hasFaceStr = (ImagetoDbResponse)facenet.GetWebResponse<ImagetoDbResponse>(request);

                if(hasFaceStr.hasFace.Equals("0"))
                    return new List<string>() { "Erro", "Não foi possível detectar a face do aluno " + code + "\n\t1- Certifique-se de que há uma face na imagem;\tCertifique-se de que altura da imagem é maior que a largura (que a imagem está em formato retrato);"};

                string procName = "addOrUpdateImage";
                List<Tuple<string, object>> parameters = new List<Tuple<string, object>>();

                parameters.Add(new Tuple<string, object>("@Code", code));
                parameters.Add(new Tuple<string, object>("@Image", imgByte));

                if(database.ExecuteProcedure(procName, parameters) <= 0)
                    return new List<string>() { "Erro", "Não foi possível inserir a imagem do aluno " + code };

                return new List<string>() { "Sucesso", "A imagem do aluno " + name + " (" + code + ") foi inserida com sucesso" };
            }catch(ResponseException ex) {
                return new List<string>() { ex.Info.header, ex.Info.message };
            }

        }
    }
}