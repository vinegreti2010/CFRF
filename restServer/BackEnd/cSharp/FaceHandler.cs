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

        /********************************************************************************
        *                                Local Debug                                    *
        ********************************************************************************/
        /*string imagesPath = "C:/Users/Administrador/Desktop/ImagesTcc/";
        string haarcascadePath = "C:/Users/Administrador/Documents/Visual Studio 2017/Projects/restServer/restServer/restServer/BackEnd/Python/Facenet/haarcascades/";
        string faceNetExec = "C:/Users/Administrador/Documents/Visual Studio 2017/Projects/restServer/restServer/restServer/BackEnd/Python/Facenet/pythonCodes/face_match_demo.py";
        string pythonPath = "C:/Python36/python.exe";
        string callMatch = " \"{0}\" --img1={1} --img2={2} --img3={3} --img4={4}";*/

        /********************************************************************************
        *                               Azure Server                                    *
        ********************************************************************************/
        string imagesPath = "D:/home/site/wwwroot/Images/";
        string haarcascadePath = "D:/home/site/wwwroot/BackEnd/Python/Facenet/haarcascades/";
        string faceNetExec = "D:/home/site/wwwroot/BackEnd/Python/Facenet/pythonCodes/face_match.py";
        string pythonPath = "D:/home/python364x64/python.exe";
        string callMatch = "{0} --img1={1} --img2={2} --img3={3} --img4={4}";

        public Face(byte[] photo) {
            ListOfFaces = new List<Bitmap>();
            MemoryStream memoryStream = new MemoryStream(photo);
            Image image = Bitmap.FromStream(memoryStream);
            image = ToPortrait(image);

            AdjustBright = 130 - MedBrightImage((Bitmap)image);
            if(AdjustBright > 0)
                image = AdjustBrightness(image, (int)(AdjustBright / 3f));

            SaveImage(image, imagesPath + "tmpImage" + 0);
            /*List<Bitmap> faces = CropFace(image, 0, false);

            if(faces != null) {
                for(int i = 0; i < faces.Count; i++) {
                    ListOfFaces.Add(faces[i]);
                }
            }*/
        }

        public bool CheckFace(byte[] photo, byte[] photo1) {
            float distance = -2;
            ResponseInfo response;

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

            try {
                string result = CompareFaces(faceNetExec, imagesPath + "BaseImage.jpg", imagesPath + "tmpImage0.jpg", imagesPath + "tmpImage1.jpg", imagesPath + "tmpImage2.jpg");

                string[] results = result.Split(' ');

                distance = float.Parse(results[0]);
                
                if(distance <= 1.1f)
                    return true;

                return false;
            } catch {
                if(distance == -1f) {
                    response = new ResponseInfo {
                        code = "3",
                        header = "Erro",
                        message = "Não foi possível detectar sua face, por favor tente novamente"
                    };
                    throw new Exception(JsonConvert.SerializeObject(response));
                } else if(distance == -2f) {
                    response = new ResponseInfo {
                        code = "4",
                        header = "Erro",
                        message = "Desculpe, não é possível validar presença com foto de outra foto"
                    };
                    throw new Exception(JsonConvert.SerializeObject(response));
                }

                response = new ResponseInfo {
                    code = "7",
                    header = "Erro",
                    message = "Erro ao carregar arquivos para o reconhecimento"
                };
                throw new Exception(JsonConvert.SerializeObject(response));
            }
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
                image = AdjustBrightness(image, (int)(threadParam.Adjust / 3f));

            if(threadParam.Index == 1) {
                SaveImage(image, imagesPath + "tmpImage" + threadParam.Index);
            } else if(threadParam.Index == 2) {
                SaveImage(image, imagesPath + "tmpImage" + threadParam.Index);
            }
            /*if(threadParam.Index == 1) {
                croppedFaces = CropFace(image, threadParam.Index, true);
            } else if(threadParam.Index == 2) {
                croppedFaces1 = CropFace(image, threadParam.Index, true);
            }*/
        }

        private Image ToPortrait(Image image) {
            Image imageAux = image;

            if(imageAux.Height < imageAux.Width)
                imageAux.RotateFlip(RotateFlipType.Rotate270FlipNone);

            return imageAux;
        }

        private List<Bitmap> CropFace(Image photo, int index, bool delete) {
            IImage image = ByteToIImage(imagesPath, photo, index, delete);
            List<Bitmap> facesList = new List<Bitmap>();
            long detectionTime;
            List<Rectangle> faces = new List<Rectangle>();
            List<Rectangle> eyes = new List<Rectangle>();
            int ind = 0;

            DetectFace.Detect(
              image, haarcascadePath + "haarcascade_frontalface_default.xml"
              , haarcascadePath + "haarcascade_eye.xml"
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

        private IImage ByteToIImage(string path, Image image, int index, bool delete) {
            if(SaveImage(image, path + "tmpImage" + index)) {
                IImage newImage = new UMat(path + "tmpImage" + index + ".jpg", ImreadModes.Color);
                if(delete)
                    File.Delete(path + "tmpImage" + index + ".jpg");

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

        private string CompareFaces(string codeName, string img1, string img2, string img3, string img4) {
            //private string CompareFaces(string codeName, string img1, string img2) {
            ProcessStartInfo start = new ProcessStartInfo {
                FileName = pythonPath,
                Arguments = string.Format(callMatch, codeName, img1, img2, img3, img4),
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