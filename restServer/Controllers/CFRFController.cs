using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Informations;
using Newtonsoft.Json;
using Presence;
using CFRFException;
using AddBaseImage;

namespace restServer.Controllers {
    public class InsertImage{
        public string code { get; set; }
        public string img { get; set; }
    }
    public class CFRFController : ApiController {
        // GET api/values
        public string Get() {
            string t = JsonConvert.SerializeObject(new ResponseInfo {
                header = "Erro",
                message = "Não foi possível validar sua presença"});
            return t;
        }

        // GET api/values/5
        public string Get(int id) {
            return "value";
        }

        [HttpPut]
        public HttpResponseMessage InsertImageOnDB([FromBody] InsertImage info) {
            //Exemplo: {"code":"111111111111", "img":"D:/home/site/wwwroot/images/coelho.jpeg"}
            if(!AddImage.Add(info.code, info.img))
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public string ValidatePresence([FromBody] JsonInformations informations) {
            ResponseInfo response = new ResponseInfo();
            if(informations != null) {
                PresenceHandler presence = new PresenceHandler(informations);
                List<string> presenceInformations = null;
                List<Tuple<string, object>> logParameters = new List<Tuple<string, object>>();
                try {
                    presenceInformations = presence.ValidatePresence();
                    if(presenceInformations != null) {
                        response = new ResponseInfo {
                            header = "Sucesso",
                            message = presenceInformations[0] + " sua presença na aula " + presenceInformations[1] + " validada com sucesso"
                        };

                        logParameters.Add(new Tuple<string, object>("@Sucess", "Y"));
                        logParameters.Add(new Tuple<string, object>("@Error", ""));
                        presence.InsertLog(logParameters);
                        logParameters.Clear();
                        return JsonConvert.SerializeObject(response);
                    } else {
                        response = new ResponseInfo {
                            header = "Erro",
                            message = "Não foi possível validar sua presença"
                        };
                        logParameters.Add(new Tuple<string, object>("@Sucess", "N"));
                        logParameters.Add(new Tuple<string, object>("@Error", "Não foi possível validar sua presença"));
                        presence.InsertLog(logParameters);
                        logParameters.Clear();
                        return JsonConvert.SerializeObject(response);
                    }
                } catch(ResponseException e) {
                    logParameters.Add(new Tuple<string, object>("@Sucess", "N"));
                    logParameters.Add(new Tuple<string, object>("@Error", e.Info.message));
                    presence.InsertLog(logParameters);
                    logParameters.Clear();
                    return JsonConvert.SerializeObject(e.Info);
                } catch {
                    response = new ResponseInfo {
                        header = "Erro",
                        message = "Ocorreu um erro interno, favor entrar em contato com o suporte"
                    };
                    logParameters.Add(new Tuple<string, object>("@Sucess", "N"));
                    logParameters.Add(new Tuple<string, object>("@Error", "Ocorreu um erro interno, favor entrar em contato com o suporte"));
                    presence.InsertLog(logParameters);
                    logParameters.Clear();
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