using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Collections.Generic;
using System.Threading;
using FaceDetection;
using Informations;
using Newtonsoft.Json;

public class ThreadParam {
    public byte[] Photo { get; set; }
    public int Index { get; set; }
    public int Adjust { get; set; }

    public ThreadParam(byte[] photo, int index, int adjust) {
        this.Photo = photo;
        this.Index = index;
        this.Adjust = adjust;
    }
}

namespace FaceHandler {
    public class Face {
        List<Bitmap> ListOfFaces;
        List<Bitmap> croppedFaces;
        List<Bitmap> croppedFaces1;
        int AdjustBright;
        //string path = "C:\\Users\\Administrador\\Documents\\Visual Studio 2017\\Projects\\restServer\\restServer\\";
        //string path = "C:\\Users\\Administrador\\Desktop\\ImagesTcc\\";
        //string path = "C:/Users/Administrador/Desktop/ImagesTcc/";
		string path = "D:/home/site/wwwroot/ImagesTcc/";
		
        public Face(byte[] photo) {
            ListOfFaces = new List<Bitmap>();
            MemoryStream memoryStream = new MemoryStream(photo);
            Image image = Bitmap.FromStream(memoryStream);
            image = ToPortrait(image);

            AdjustBright = 130 - MedBrightImage((Bitmap)image);
            if(AdjustBright > 0)
                image = AdjustBrightness(image, (int)(AdjustBright / 3f));

            List<Bitmap> faces = CropFace(image, 0);

            if(faces != null) {
                for(int i = 0; i < faces.Count; i++) {
                    ListOfFaces.Add(faces[i]);
                }
            }
        }

        public bool CheckFace() {
            string result = CompareFaces("D:/home/site/wwwroot/facematch/face_match.py", path + "BaseImage.jpg", path + "tmpImage0.jpg");
            //string result = CompareFaces("C:/Users/Administrador/Documents/Visual Studio 2017/Projects/restServer/restServer/restServer/facematch/face_match_demo.py", path + "BaseImage.jpg", path + "tmpImage0.jpg");

            string[] results = result.Split(' ');

            float distance = float.Parse(results[0]);

            if (distance <= 1.1f)
                return true;

            return false;
        }

        public bool IsPhotoOfPhoto(byte[] photo, byte[] photo1) {
            croppedFaces = new List<Bitmap>();
            croppedFaces1 = new List<Bitmap>();

            Thread t1 = new Thread(new ParameterizedThreadStart(ThreadAdjsutImage));
            t1.Name = "Photos 1";
            ThreadParam param = new ThreadParam(photo, 1, AdjustBright);
            t1.Start(param);

            Thread t2 = new Thread(new ParameterizedThreadStart(ThreadAdjsutImage));
            t2.Name = "Photos 2";
            param = new ThreadParam(photo1, 2, AdjustBright);
            t2.Start(param);

            t1.Join();
            t2.Join();

            if(croppedFaces.Count <= 0 || croppedFaces1.Count <= 0 || ListOfFaces.Count <= 0) {
                ResponseInfo response = new ResponseInfo {
                    code = "3",
                    header = "Erro",
                    message = "Não foi possível detectar sua face, por favor tente novamente"
                };
                throw new Exception(JsonConvert.SerializeObject(response));
            }

            if(!IsImagesCompletelyEqual(croppedFaces, croppedFaces1, ListOfFaces))
                return false;

            return true;
        }

        private void ThreadAdjsutImage(object param) {
            ThreadParam threadParam = (ThreadParam)param;

            MemoryStream memoryStream = new MemoryStream(threadParam.Photo);
            Image image = Bitmap.FromStream(memoryStream);
            image = ToPortrait(image);
            if(threadParam.Adjust > 0)
                image = AdjustBrightness(image, (int)(threadParam.Adjust/3f));
            if(threadParam.Index == 1) {
                croppedFaces = CropFace(image, threadParam.Index);
            } else if(threadParam.Index == 2) {
                croppedFaces1 = CropFace(image, threadParam.Index);
            }
        }

        private Image ToPortrait(Image image) {
            Image imageAux = image;

            if(imageAux.Height < imageAux.Width)
                imageAux.RotateFlip(RotateFlipType.Rotate270FlipNone);

            return imageAux;
        }

        private List<Bitmap> CropFace(Image photo, int index) {
            IImage image = ByteToIImage(photo, index);
            List<Bitmap> facesList = new List<Bitmap>();
            long detectionTime;
            List<Rectangle> faces = new List<Rectangle>();
            List<Rectangle> eyes = new List<Rectangle>();
            int ind = 0;
			
			/*DetectFace.Detect(
              image, "C:\\Users\\Administrador\\Documents\\Visual Studio 2017\\Projects\\restServer\\restServer\\restServer\\BackEnd\\haarcascade_frontalface_default.xml"
              , "C:\\Users\\Administrador\\Documents\\Visual Studio 2017\\Projects\\restServer\\restServer\\restServer\\BackEnd\\haarcascade_eye.xml"
              , faces, eyes,
              out detectionTime);*/
			  
            DetectFace.Detect(
              image, "D:\\home\\site\\wwwroot\\BackEnd\\haarcascade_frontalface_default.xml"
              , "D:\\home\\site\\wwwroot\\BackEnd\\haarcascade_eye.xml"
              , faces, eyes,
              out detectionTime);

            foreach(Rectangle face in faces) {
                CvInvoke.Rectangle(image, face, new Bgr(Color.Red).MCvScalar, 2);

                Bitmap target = new Bitmap(face.Width, face.Height);
                Bitmap src = image.Bitmap;
                using(Graphics g = Graphics.FromImage(target)) {
                    g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                                     face,
                                     GraphicsUnit.Pixel);
                }
                facesList.Add(target);
                ind += 1;
            }

            return facesList;
        }

        private bool SaveImage(Image image, string fileName) {
            try {
                try {
                    File.Delete(fileName + ".jpg");
                } catch { }
                image.Save(fileName + ".jpg", ImageFormat.Jpeg);
                return true;
            } catch(Exception) {
                return false;
            }
        }

        private IImage ByteToIImage(Image image, int index) {
            if(SaveImage(image, path + "tmpImage" + index)) {
                IImage newImage = new UMat(path + "tmpImage" + index + ".jpg", ImreadModes.Color);
                //File.Delete("tmpImage.jpg");
                return newImage;
            }
            return null;
        }

        private int MedBrightImage(Bitmap image) {
            Color color;
            int count = 0;
            float sumBright = 0;

            for(int i = 0; i < image.Width - 1; i++) {
                for(int j = 0; j < image.Height; j++) {
                    color = image.GetPixel(i, j);
                    sumBright += color.GetBrightness();
                    count++;
                }
            }

            return count == 0 ? 0 : (int)(sumBright * 255) / count;
        }

        private Image AdjustBrightness(Image image, int Value) {
            Image TempBitmap = image;
            Image NewBitmap = new Bitmap(TempBitmap.Width, TempBitmap.Height);
            Graphics NewGraphics = Graphics.FromImage(NewBitmap);
            float FinalValue = (float)Value / 255.0f;
            float[][] FloatColorMatrix ={
                    new float[] {1, 0, 0, 0, 0},
                    new float[] {0, 1, 0, 0, 0},
                    new float[] {0, 0, 1, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {FinalValue, FinalValue, FinalValue, 1, 1}
                };

            ColorMatrix NewColorMatrix = new ColorMatrix(FloatColorMatrix);
            ImageAttributes Attributes = new ImageAttributes();

            Attributes.SetColorMatrix(NewColorMatrix);
            NewGraphics.DrawImage(TempBitmap, new Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), 0, 0, TempBitmap.Width, TempBitmap.Height, GraphicsUnit.Pixel, Attributes);
            Attributes.Dispose();
            NewGraphics.Dispose();

            return NewBitmap;
        }

        private bool IsImagesCompletelyEqual(List<Bitmap> f1, List<Bitmap> f2, List<Bitmap> f) {

            for(int i = 0; i < f1.Count; i++) {
                for(int j = 0; j < f2.Count; j++) {
                    for(int k = 0; k < f.Count; k++) {
                        if(f1[i] == f2[j] && f1[i] == f[k])
                            return true;
                    }
                }
            }

            return false;
        }
        //FileName = "C:/Python36/python.exe",
        private string CompareFaces(string codeName, string img1, string img2) {
            ProcessStartInfo start = new ProcessStartInfo {
                FileName = "D:/home/python364x64/python.exe",
                Arguments = string.Format(" \"{0}\" --img1={1} --img2={2}", codeName, img1, img2),
                UseShellExecute = false,// Do not use OS shell
                CreateNoWindow = true, // We don't need new window
                RedirectStandardOutput = true,// Any output, generated by application will be redirected back
                RedirectStandardError = true // Any error in standard output will be redirected back (for example exceptions)
            };
            using(Process process = Process.Start(start)) {
                using(StreamReader reader = process.StandardOutput) {
                    string stderr = process.StandardError.ReadToEnd(); // Here are the exceptions from our Python script
                    string result = reader.ReadToEnd(); // Here is the result of StdOut(for example: print "test")
                    return result;
                }
            }
        }
    }
}