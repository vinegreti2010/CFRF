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
        List<object> queryNameImage;
        List<object> queryLocation;
        DatabaseHandler database;
        double facesDistance, timeResponse;
        public PresenceHandler(JsonInformations informations) {
            this.Informations = informations;
            database = Singleton<DatabaseHandler>.Instance();
        }

        public List<string> ValidatePresence() {
            Exception exceptionRecognize = null, exceptionLocation = null;

            string query = "SELECT A.name_display, B.student_image FROM personal_data A INNER JOIN student_images B ON B.student_id = A.student_id WHERE A.student_id = '" + Informations.Code + "';";
            queryNameImage = database.ExecuteQuery(query);

            if(queryNameImage.Count == 0)
                throw new ResponseException("Erro", "Desculpe, não existe foto cadastrada para o código inserido");

            query = "SELECT C.class_nbr, C.strm, C.attend_dt, C.start_time, B.descr, D.latitude_north_east, D.longitude_north_east, D.latitude_north_west, D.longitude_north_west, D.latitude_south_east, D.longitude_south_east, D.latitude_south_west, D.longitude_south_west FROM stdnt_enrl A INNER JOIN class_tbl B ON B.class_nbr = A.class_nbr AND B.strm = A.strm INNER JOIN class_attendence C ON C.class_nbr = A.class_nbr AND C.strm = A.strm AND C.student_id = A.student_id LEFT JOIN facility_tbl D ON D.facility_id = B.facility_id WHERE A.student_id = '" + Informations.Code + "' AND C.attend_dt = CONVERT(DATE, GETDATE()) AND CONVERT(TIME, GETDATE()) BETWEEN C.start_time AND C.end_time;";
            queryLocation = database.ExecuteQuery(query);

            if(queryLocation.Count == 0)
                throw new ResponseException("Erro", "Desculpe, sua matrícula não foi encontrada para esse horário");

            if(queryLocation.Count < 13)
                throw new ResponseException("Erro", "As coordenadas para a sala onde você tem aula não foram cadastradas");

            Thread recognizeThread = new Thread(() => SafeExecute(() => RecognizeFace(), out exceptionRecognize));
            recognizeThread.Start();

            Thread locationThread = new Thread(() => SafeExecute(() => CheckLocation(), out exceptionLocation));
            locationThread.Start();

            recognizeThread.Join();
            locationThread.Join();

            if(!isFaceCorrect) {
                if(exceptionRecognize != null) {
                    InsertRecognizeLog("N", exceptionRecognize.Message, timeResponse, facesDistance);
                    throw exceptionRecognize;
                }
                InsertRecognizeLog("N", "Desculpe, a foto não corresponde com a foto de referencia para este código", timeResponse, facesDistance);

                throw new ResponseException("Erro", "Desculpe, a foto não corresponde com a foto de referencia para este código");
            }

            InsertRecognizeLog("Y", "", timeResponse, facesDistance);

            if(!isFacilityCorrect) {
                if(exceptionLocation != null)
                    throw exceptionLocation;
                throw new ResponseException("Erro", "Desculpe, suas coordenadas não correspondem à sala onde você tem aula neste horário");
            }

            if(!ApplyPresence(queryLocation[0], queryLocation[1], Informations.Code, queryLocation[2], queryLocation[3])){
                throw new ResponseException("Erro", "Não foi possível atualizar sua presença no banco de dados, favor entrar em contato com o administrador");
            }
            
            return new List<string>() { (string)queryNameImage[0], (string) queryLocation[4]};
        }

        private void RecognizeFace() {
            Face face = new Face(this.Informations.Code, this.Informations.Photo);
            FacenetResponseInformations facenetResponse = face.CheckFace((byte[])this.queryNameImage[1], this.Informations.Photo1, this.Informations.Photo2);
            if(facenetResponse.distance <= 1.1f)
                isFaceCorrect = true;
            else
                isFaceCorrect = false;

            facesDistance = facenetResponse.distance;
            timeResponse = facenetResponse.time;
        }

        private void CheckLocation() {
            Location location = new Location(Informations.Latitude, Informations.Longitude, Informations.Accuracy);

            isFacilityCorrect = location.CheckCoordinate((float)((decimal)this.queryLocation[5]),
                                                         (float)((decimal)this.queryLocation[6]),
                                                         (float)((decimal)this.queryLocation[7]),
                                                         (float)((decimal)this.queryLocation[8]),
                                                         (float)((decimal)this.queryLocation[9]),
                                                         (float)((decimal)this.queryLocation[10]),
                                                         (float)((decimal)this.queryLocation[11]),
                                                         (float)((decimal)this.queryLocation[12]));
        }

        private static void SafeExecute(Action action, out Exception exception) {
            exception = null;
            try {
                action.Invoke();
            } catch(Exception e) {
                exception = e;
            }
        }

        private bool ApplyPresence(object classNbr, object strm, object code, object attendDt, object startTime) {
            string procName = "updatePresence";
            List<Tuple<string, object>> parameters = new List<Tuple<string, object>>();

            parameters.Add(new Tuple<string, object>("@Class_nbr", classNbr));
            parameters.Add(new Tuple<string, object>("@Strm", strm));
            parameters.Add(new Tuple<string, object>("@Student_id", code));
            parameters.Add(new Tuple<string, object>("@Attend_dt", attendDt));
            parameters.Add(new Tuple<string, object>("@Start_time", startTime));

            if(database.ExecuteProcedure(procName, parameters) > 0)
                return true;

            return false;
        }

        private void InsertRecognizeLog(string Sucess, string Error, double timeResponse, double distance) {
            List<Tuple<string, object>> parameters = new List<Tuple<string, object>>();
            parameters.Add(new Tuple<string, object>("@Student_id", Informations.Code));
            parameters.Add(new Tuple<string, object>("@Sucess", Sucess));
            parameters.Add(new Tuple<string, object>("@Error", Error));
            parameters.Add(new Tuple<string, object>("@Time", timeResponse));
            parameters.Add(new Tuple<string, object>("@Distance", distance));

            database.ExecuteProcedure("insertRecognizeLog", parameters);
        }

        public void InsertLog(List<Tuple<string, object>> parameters) {
            parameters.Add(new Tuple<string, object>("@Student_id", Informations.Code));
            database.ExecuteProcedure("insertLog", parameters);
        }
    }
}