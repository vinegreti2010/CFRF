﻿namespace Informations{
    public class JsonInformations {
        public string Code { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public float Accuracy { get; set; }
        public byte[] Photo { get; set; }
        public byte[] Photo1 { get; set; }
        public byte[] Photo2 { get; set; }
    }

    public class ResponseInfo {
        public string header { get; set; }
        public string message { get; set; }
    }

    public class FacenetRequestInformations {
        public string baseImage { get; set; }
        public string img1 { get; set; }
        public string img2 { get; set; }
        public string img3 { get; set; }

    }

    public class FacenetResponseInformations {
        public float distance { get; set; }
        public float time { get; set; }
    }
}