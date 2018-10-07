using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Database;
using Informations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Presence;
using CFRFException;

namespace restServer.Controllers {
    public class InsertImage{
        public string code { get; set; }
        public string img { get; set; }
    }
    public class CFRFController : ApiController {
        // GET api/values
        public string Get() {
            string t = JsonConvert.SerializeObject(new ResponseException("teste", "teste").Info);
            return t;
        }

        // GET api/values/5
        public string Get(int id) {
            return "value";
        }

        [HttpPut]
        public HttpResponseMessage InsertImageOnDB([FromBody] InsertImage info) {
            Image img = new Bitmap(info.img);
            MemoryStream memoryStream = new MemoryStream();
            img.Save(memoryStream, ImageFormat.Jpeg);
            byte[] imgByte = memoryStream.ToArray();

            DatabaseHandler database = Singleton<DatabaseHandler>.Instance();
            string procName = "addimage";
            database.InsertByProc(procName, info.code, imgByte);
            database.InsertByProc(procName, info.code, imgByte);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public string ValidatePresence([FromBody] JsonInformations informations) {
            ResponseInfo response = new ResponseInfo();
            if(informations != null) {
                PresenceHandler presence = new PresenceHandler(informations);
                List<string> presenceInformations = null;
                try {
                    presenceInformations = presence.ValidatePresence();
                    if(presenceInformations != null) {
                        response = new ResponseInfo {
                            header = "Sucesso",
                            message = presenceInformations[0] + " sua presença na aula " + presenceInformations[1] + " validada com sucesso"
                        };
                        return JsonConvert.SerializeObject(response);
                    } else {
                        response = new ResponseInfo {
                            header = "Erro",
                            message = "Não foi possível validar sua presença"
                        };
                        return JsonConvert.SerializeObject(response);
                    }
                } catch(ResponseException e) {
                    return JsonConvert.SerializeObject(e.Info);
                } catch {
                    response = new ResponseInfo {
                        header = "Erro",
                        message = "Ocorreu um erro interno, favor entrar em contato com o suporte"
                    };
                    return JsonConvert.SerializeObject(response);
                }
            } else {
                response = new ResponseInfo {
                    header = "Erro",
                    message = "Informações inválidas recebidas pelo servidor"
                };
                return JsonConvert.SerializeObject(response);
            }
        }
    }
}