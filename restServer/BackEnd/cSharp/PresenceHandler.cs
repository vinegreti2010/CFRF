using Informations;
using LocationHandler;
using FaceHandler;
using System;
using Database;
using System.Collections.Generic;
using System.Threading;
using CFRFException;

namespace Presence {
    public class PresenceHandler {
        private bool isFaceCorrect = false, isFacilityCorrect = false;
        JsonInformations Informations;
        List<object> queryInformations;
        DatabaseHandler database;
        public PresenceHandler(JsonInformations informations) {
            this.Informations = informations;
            database = Singleton<DatabaseHandler>.Instance();
        }

        public bool ValidatePresence() {
            try {
                string query = "SELECT B.descr, D.latitude_north_east, D.longitude_north_east, D.latitude_north_west, D.longitude_north_west, D.latitude_south_east, D.longitude_south_east, D.latitude_south_west, D.longitude_south_west, E.name_display, F.student_image FROM stdnt_enrl A INNER JOIN class_tbl B ON B.class_nbr = A.class_nbr AND B.strm = A.strm INNER JOIN class_attendence C ON C.class_nbr = A.class_nbr AND C.strm = A.strm AND C.student_id = A.student_id INNER JOIN facility_tbl D ON D.facility_id = B.facility_id INNER JOIN personal_data E ON E.student_id = A.student_id INNER JOIN student_images F ON F.student_id = A.student_id WHERE A.student_id = '555555555555' AND C.attend_dt = CONVERT(DATE, '20180820 19:15:00 PM') AND CONVERT(TIME, '20180820 19:15:00 PM') BETWEEN C.start_time AND C.end_time;";
                queryInformations = database.ExecuteQuery(query);

                //Thread recognizeThread = new Thread(RecognizeFace);
                Exception exception = null;
                Thread recognizeThread = new Thread(() => SafeExecute(() => RecognizeFace(), out exception));
                recognizeThread.Start();


                recognizeThread.Join();
                
                if(!isFaceCorrect)
                    throw new ResponseException("Erro", "Desculpe, a foto não corresponde com a foto de referencia para este código");

                if(!isFacilityCorrect)
                    throw new ResponseException("Erro", "Desculpe, suas coordenadas não correspondem à sala onde você tem aula neste horário");

                return true;
            } catch(Exception e) {
                throw e;
            }
        }

        private void RecognizeFace() {
            Face face = new Face(this.Informations.Code, this.Informations.Photo);
            FacenetResponseInformations facenetResponse = face.CheckFace((byte[])this.queryInformations[10], this.Informations.Photo1, this.Informations.Photo2);
            if(facenetResponse.distance <= 1.1f)
                isFaceCorrect = true;
            else
                isFaceCorrect = false;
        }

        private void CheckLocation() {
            Location location = new Location(Informations.Latitude, Informations.Longitude, Informations.Accuracy);
            location.CheckCoordinate();
        }

        private static void SafeExecute(Action action, out Exception exception) {
            exception = null;
            try {
                action.Invoke();
            } catch(Exception e) {
                exception = e;
            }
        }
    }
}