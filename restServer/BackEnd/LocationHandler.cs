using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocationHandler {
    public class Location{
        private readonly float Latitude;
        private readonly float Longitude;
        private readonly float Accuracy;
        private float minLatitude;
        private float maxLatitude;
        private float minLongitude;
        private float maxLongitude;
        public Location(float latitude, float longitude, float accuracy) {
            Latitude = latitude;
            Longitude = longitude;
            Accuracy = accuracy;
        }
        public float CheckCoordinate() {

            applyAccuracy(GetDegreeAccuracy());
            
            return 1;
        }
        private float GetDegreeAccuracy() {
            //1 minuto geodésico = 1 milha nautica = 1851.997958112 metros.
            //1 segundo geodésico = 0.016666667 minutos geodésicos = 30.866632633 metros.

            //precisão em segundos
            float degreeAccuracy = Accuracy / 30.866632633f;

            //precisao em graus => precisao / 60 = precisaoMinutos -> precisaoMinutos / 60 = precisaoGraus
            return (degreeAccuracy / 360);
        }

        private void applyAccuracy(float degreeAccuracy) {
            maxLatitude = Latitude + degreeAccuracy;
            minLatitude = Latitude - degreeAccuracy;
            maxLongitude = Longitude + degreeAccuracy;
            minLongitude = Longitude - degreeAccuracy;
        }
    }
}