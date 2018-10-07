using System;
using Informations;

namespace CFRFException {
    //[Serializable]
    public class ResponseException : Exception {
        public ResponseInfo Info;

        public ResponseException(string header, string message) {
            Info = new ResponseInfo {
                header = header,
                message = message
            };
        }
    }
}