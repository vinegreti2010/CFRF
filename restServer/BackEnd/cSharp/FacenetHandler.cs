using System;
using System.IO;
using System.Net;
using Informations;
using Newtonsoft.Json;

namespace Facenet {
    public class FacenetHandler {
        private WebRequest webRequest;

        public WebRequest SendMessage(string operation, string method, FacenetRequestInformations informations) {
            string jSonString = JsonConvert.SerializeObject(informations);

            if(String.IsNullOrEmpty(jSonString)) {
                ResponseInfo response = new ResponseInfo {
                    code = "4",
                    header = "Erro",
                    message = "Não foi possível enviar as informações para servidor de reconhecimento"
                };
                throw new Exception(JsonConvert.SerializeObject(response));
            }

            try {
                PreparaWebRequest("facenet/" + operation, method);

                using(var streamWriter = new StreamWriter(webRequest.GetRequestStream())) {
                    streamWriter.Write(jSonString);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                return webRequest;
            } catch {
                ResponseInfo response = new ResponseInfo {
                    code = "4",
                    header = "Erro",
                    message = "Não foi possível enviar as informações para servidor de reconhecimento"
                };
                throw new Exception(JsonConvert.SerializeObject(response));
            }
        }

        private void PreparaWebRequest(string operacao, string metodo) {
            string url = "http://127.0.0.1:5000/";
            try {
                webRequest = WebRequest.Create(url + operacao);
                webRequest.Method = metodo;
                webRequest.ContentType = "application/json; charset=utf-8";
            } catch {
                ResponseInfo response = new ResponseInfo {
                    code = "4",
                    header = "Erro",
                    message = "Servidor de reconhecimento inacessível, favor contatar o suporte"
                };
                throw new Exception(JsonConvert.SerializeObject(response));
            }
        }

        public FacenetResponseInformations GetWebResponse(WebRequest webRequest) {
            try {
                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

                var responseText = new StreamReader(response.GetResponseStream()).ReadToEnd();
                //responseText = responseText.Substring(0, responseText.Length - 1);
                responseText = responseText.Replace("}\n", "}");
                FacenetResponseInformations responseInfo = JsonConvert.DeserializeObject<FacenetResponseInformations>(responseText);
                return responseInfo;
            } catch {
                ResponseInfo response = new ResponseInfo {
                    code = "4",
                    header = "Erro",
                    message = "Não foi receber resopsta do servidor de reconhecimento"
                };
                
                throw new Exception(JsonConvert.SerializeObject(response));
            } finally {
                webRequest.Abort();
            }
        }
    }
}