﻿using Informations;
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

            query = "SELECT B.descr, D.latitude_north_east, D.longitude_north_east, D.latitude_north_west, D.longitude_north_west, D.latitude_south_east, D.longitude_south_east, D.latitude_south_west, D.longitude_south_west FROM stdnt_enrl A INNER JOIN class_tbl B ON B.class_nbr = A.class_nbr AND B.strm = A.strm INNER JOIN class_attendence C ON C.class_nbr = A.class_nbr AND C.strm = A.strm AND C.student_id = A.student_id LEFT JOIN facility_tbl D ON D.facility_id = B.facility_id WHERE A.student_id = '" + Informations.Code + "' AND C.attend_dt = CONVERT(DATE, GETDATE()) AND CONVERT(TIME, GETDATE()) BETWEEN C.start_time AND C.end_time;";
            queryLocation = database.ExecuteQuery(query);

            if(queryLocation.Count == 0)
                throw new ResponseException("Erro", "Desculpe, sua matrícula não foi encontrada para esse horário");

            if(queryLocation.Count < 9)
                throw new ResponseException("Erro", "As coordenadas para a sala onde você tem aula não foram cadastradas");

            Thread recognizeThread = new Thread(() => SafeExecute(() => RecognizeFace(), out exceptionRecognize));
            recognizeThread.Start();

            Thread locationThread = new Thread(() => SafeExecute(() => CheckLocation(), out exceptionLocation));
            locationThread.Start();

            recognizeThread.Join();
            locationThread.Join();

            if(!isFaceCorrect) {
                if(exceptionRecognize != null)
                    throw exceptionRecognize;
                throw new ResponseException("Erro", "Desculpe, a foto não corresponde com a foto de referencia para este código");
            }

            if(!isFacilityCorrect) {
                if(exceptionLocation != null)
                    throw exceptionLocation;
                throw new ResponseException("Erro", "Desculpe, suas coordenadas não correspondem à sala onde você tem aula neste horário");
            }

            return new List<string>() { (string)queryNameImage[0], (string) queryLocation[0]};
        }

        private void RecognizeFace() {
            Face face = new Face(this.Informations.Code, this.Informations.Photo);
            FacenetResponseInformations facenetResponse = face.CheckFace((byte[])this.queryNameImage[1], this.Informations.Photo1, this.Informations.Photo2);
            if(facenetResponse.distance <= 1.1f)
                isFaceCorrect = true;
            else
                isFaceCorrect = false;
        }

        private void CheckLocation() {
            Location location = new Location(Informations.Latitude, Informations.Longitude, Informations.Accuracy);

            isFacilityCorrect = location.CheckCoordinate((float)((decimal)this.queryLocation[1]),
                                                         (float)((decimal)this.queryLocation[2]),
                                                         (float)((decimal)this.queryLocation[3]),
                                                         (float)((decimal)this.queryLocation[4]),
                                                         (float)((decimal)this.queryLocation[5]),
                                                         (float)((decimal)this.queryLocation[6]),
                                                         (float)((decimal)this.queryLocation[7]),
                                                         (float)((decimal)this.queryLocation[8]));
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