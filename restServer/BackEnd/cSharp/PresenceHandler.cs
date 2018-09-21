using Informations;
using LocationHandler;
using FaceHandler;
using Newtonsoft.Json;
using System;

namespace Presence {
    public class PresenceHandler {
        public bool ValidatePresence(JsonInformations informations) {
            try {
                Location location = new Location(informations.Latitude, informations.Longitude, informations.Accuracy);
                location.CheckCoordinate();

                Face face = new Face(informations.Photo);
                
                /*
                if(face.IsPhotoOfPhoto(informations.Photo1, informations.Photo2)) {
                    ResponseInfo response = new ResponseInfo {
                        code = "4",
                        header = "Erro",
                        message = "Desculpe, não é possível validar presença com foto de outra foto"
                    };
                    throw new Exception(JsonConvert.SerializeObject(response));
                }
                */
                if(face.CheckFace(informations.Photo1, informations.Photo2))
                    return true;
                else {
                    ResponseInfo response = new ResponseInfo {
                        code = "5",
                        header = "Erro",
                        message = "Desculpe, a foto não corresponde com a foto de referencia para este código"
                    };
                    throw new Exception(JsonConvert.SerializeObject(response));
                }
            } catch(Exception e) {
                throw e;
            }
        }
    }
}