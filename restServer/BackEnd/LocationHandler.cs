namespace LocationHandler {
    public class Location{
        private readonly float latitude;
        private readonly float longitude;
        private readonly float accuracy;
        private float minLatitude;
        private float maxLatitude;
        private float minLongitude;
        private float maxLongitude;

        public Location(float latitude, float longitude, float accuracy) {
            this.latitude = latitude;
            this.longitude = longitude;
            this.accuracy = accuracy;
        }

        public bool CheckCoordinate(float latitude_north_east, float longitude_north_east, float latitude_north_west, float longitude_north_west, float latitude_south_east, float longitude_south_east, float latitude_south_west, float longitude_south_west) {
            if((latitude <= latitude_north_east || latitude <= latitude_north_west) && (latitude >= latitude_south_east || latitude >= latitude_south_west))
                if((longitude <= longitude_north_east || longitude <= longitude_south_east) && (longitude >= longitude_north_west || longitude>= longitude_south_west))
                    return true;

            applyAccuracy(GetDegreeAccuracy());

            if((minLatitude <= latitude_north_east || minLatitude <= latitude_north_west) && (minLatitude >= latitude_south_east || minLatitude >= latitude_south_west))
                if((minLongitude <= longitude_north_east || minLongitude <= longitude_south_east) && (minLongitude >= longitude_north_west || minLongitude >= longitude_south_west))
                    return true;

            if((maxLatitude <= latitude_north_east || maxLatitude <= latitude_north_west) && (maxLatitude >= latitude_south_east || maxLatitude >= latitude_south_west))
                if((maxLongitude<= longitude_north_east || maxLongitude <= longitude_south_east) && (maxLongitude >= longitude_north_west || maxLongitude >= longitude_south_west))
                    return true;

            return false;
        }

        private float GetDegreeAccuracy() {
            //1 minuto geodésico = 1 milha nautica = 1851.997958112 metros.
            //1 segundo geodésico = 0.016666667 minutos geodésicos = 30.866632633 metros.
            //precisao em graus => precisaoMetros / 30.866632633 -> precisaoSegundos / 60 = precisaoMinutos -> precisaoMinutos / 60 = precisaoGraus
            //precisao em graus = precisaoMetros / 111119.877478800
            return accuracy / 111119.877478800f;
        }

        private void applyAccuracy(float degreeAccuracy) {
            maxLatitude = this.latitude + degreeAccuracy;
            minLatitude = this.latitude - degreeAccuracy;
            maxLongitude = this.longitude + degreeAccuracy;
            minLongitude = this.longitude - degreeAccuracy;
        }
    }
}