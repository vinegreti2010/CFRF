using System;
using System.IO;
using System.Net;
using CFRFException;
using Informations;
using Newtonsoft.Json;

namespace Facenet {
    public class FacenetHandler {
        private WebRequest webRequest;

        public WebRequest SendMessage(string operation, string method, object informations) {
            string jSonString = JsonConvert.SerializeObject(informations);

            if(String.IsNullOrEmpty(jSonString)) 
                throw new ResponseException("Erro", "Não foi possível enviar as informações para servidor de reconhecimento");

            try {
                PreparaWebRequest("facenet/" + operation, method);

                using(var streamWriter = new StreamWriter(webRequest.GetRequestStream())) {
                    streamWriter.Write(jSonString);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                return webRequest;
            } catch {
                throw new ResponseException("Erro", "Não foi possível enviar as informações para servidor de reconhecimento");
            }
        }

        private void PreparaWebRequest(string operacao, string metodo) {
            string url = "https://facenetserver.azurewebsites.net/";
            try {
                webRequest = WebRequest.Create(url + operacao);
                webRequest.Method = metodo;
                webRequest.ContentType = "application/json; charset=utf-8";
            } catch {
                throw new ResponseException("Erro", "Servidor de reconhecimento inacessível, favor contatar o suporte");
            }
        }

        public object GetWebResponse<T>(WebRequest webRequest) {
            try {
                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

                var responseText = new StreamReader(response.GetResponseStream()).ReadToEnd();
                responseText = responseText.Replace("}\n", "}");
                T responseInfo = JsonConvert.DeserializeObject<T>(responseText);
                return responseInfo;
            } catch {
                throw new ResponseException("Erro", "Não foi receber resopsta do servidor de reconhecimento");
            } finally {
                webRequest.Abort();
            }
        }
    }
}