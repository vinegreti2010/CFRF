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

        // POST api/values
        /*public void Post([FromBody]string value) {
            int i = 0;
        }*/

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

                try {
                    if(presence.ValidatePresence()) {
                        response = new ResponseInfo {
                            header = "Sucesso",
                            message = "Presença validada com sucesso"
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

        // PUT api/values/5
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE api/values/5
        public void Delete(int id) {
        }
    }
}