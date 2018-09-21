using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Informations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Presence;

namespace restServer.Controllers {
    public class CFRFController : ApiController {
        // GET api/values
        public IEnumerable<string> Get() {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id) {
            return "value";
        }

        // POST api/values
        /*public void Post([FromBody]string value) {
            int i = 0;
        }*/

        /*[HttpPost]
        public HttpResponseMessage Ex(Teste teste) {
            string t = teste.T1;
            if(t == "teste")
                return new HttpResponseMessage(HttpStatusCode.OK);
            else
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }*/

        [HttpPost]
        public string ReceiveInformations([FromBody] JsonInformations informations) {
            ResponseInfo response = new ResponseInfo();
            if(informations != null) {
                PresenceHandler presence = new PresenceHandler();
                
                try {
                    if(presence.ValidatePresence(informations)) {
                        response.code = "1";
                        response.header = "Sucesso";
                        response.message = "Presença validada com sucesso";
                        return JsonConvert.SerializeObject(response);
                    } else {
                        response.code = "6";
                        response.header = "Erro";
                        response.message = "Presença não validada";
                        return JsonConvert.SerializeObject(response);
                    }
                } catch (Exception e) {
                    string message = e.Message.Replace("\\", "");
                    response = JsonConvert.DeserializeObject<ResponseInfo>(message);
                    return JsonConvert.SerializeObject(response);
                }
            } else {
                response.code = "2";
                response.header = "Erro";
                response.message = "Informações inválidas recebidas pelo servidor";
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