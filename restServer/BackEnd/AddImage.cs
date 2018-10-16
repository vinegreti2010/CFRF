using Database;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Facenet;
using System.Net;
using Informations;

namespace AddBaseImage {
    public class AddImage {
        public static bool Add(string code, string filename) {
            Image img = new Bitmap(filename);
            MemoryStream memoryStream = new MemoryStream();
            img.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] imgByte = memoryStream.ToArray();

            FacenetHandler facenet = new FacenetHandler();
            WebRequest request = facenet.SendMessage("hasFace", "POST", new ImagetoDb() { img = Convert.ToBase64String(imgByte) });
            ImagetoDbResponse hasFaceStr = (ImagetoDbResponse)facenet.GetWebResponse<ImagetoDbResponse>(request);

            if(hasFaceStr.hasFace.Equals("0"))
                return false;

            DatabaseHandler database = Singleton<DatabaseHandler>.Instance();
            string procName = "addOrUpdateImage";
            List<Tuple<string, object>> parameters = new List<Tuple<string, object>>();

            parameters.Add(new Tuple<string, object>("@Code", code));
            parameters.Add(new Tuple<string, object>("@Image", imgByte));

            if(database.ExecuteProcedure(procName, parameters) <= 0)
                return false;

            return true;
        }
    }
}