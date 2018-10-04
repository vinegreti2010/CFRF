using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Threading;
using Informations;
using Newtonsoft.Json;
using CFRFException;
using System.Net;
using Facenet;

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
        int AdjustBright;
        string ImagesPathWithPrefix;

        /********************************************************************************
        *                                Local Debug                                    *
        ********************************************************************************/
        string imagesPath = "C:/Users/Administrador/Desktop/ImagesTcc/";

        /********************************************************************************
        *                               Azure Server                                    *
        ********************************************************************************/
        //string imagesPath = "D:/home/site/wwwroot/Images/";

        public Face(string studentCode, byte[] photo) {
            ImagesPathWithPrefix = imagesPath + studentCode;
            ListOfFaces = new List<Bitmap>();
            MemoryStream memoryStream = new MemoryStream(photo);
            Image image = Bitmap.FromStream(memoryStream);
            image = ToPortrait(image);
            
            AdjustBright = 130 - MedBrightImage((Bitmap)image);
            if(AdjustBright > 0)
                image = AdjustBrightness(image, (int)(AdjustBright / 3f));

            SaveImage(image, ImagesPathWithPrefix + "tmpImage" + 0);
        }

        public FacenetResponseInformations CheckFace(byte[] baseImage, byte[] photo, byte[] photo1) {
            Thread t1 = new Thread(new ParameterizedThreadStart(ThreadAdjsutImage));
            t1.Name = "Photos 1";
            ThreadParam param = new ThreadParam(photo, 1, AdjustBright);
            t1.Start(param);

            Thread t2 = new Thread(new ParameterizedThreadStart(ThreadAdjsutImage));
            t2.Name = "Photos 2";
            param = new ThreadParam(photo1, 2, AdjustBright);
            t2.Start(param);

            MemoryStream memoryStream = new MemoryStream(baseImage);
            Image image = Bitmap.FromStream(memoryStream);
            SaveImage(image, ImagesPathWithPrefix + "BaseImage");

            t1.Join();
            t2.Join();

            float distance = -10, time = -1;

            try {
                FacenetResponseInformations facenetResponse = RecognizeFace(ImagesPathWithPrefix + "BaseImage.jpg", ImagesPathWithPrefix + "tmpImage0.jpg", ImagesPathWithPrefix + "tmpImage1.jpg", ImagesPathWithPrefix + "tmpImage2.jpg");
                //FacenetResponseInformations facenetResponse = RecognizeFace(imagesPath + "BaseImage.jpg", ImagesPathWithPrefix + "tmpImage0.jpg", ImagesPathWithPrefix + "tmpImage1.jpg", ImagesPathWithPrefix + "tmpImage2.jpg");

                distance = facenetResponse.distance;
                time = facenetResponse.time;

                if(distance < 0.0f)
                    throw new Exception();

                return facenetResponse;

                /*if(distance <= 1.1f)
                    return true;

                return false;*/
            } catch(Exception e) {
                if(distance == -1f) 
                    throw new ResponseException("Erro", "Não foi possível detectar sua face, por favor tente novamente");
                
                if(distance == -2f) 
                    throw new ResponseException("Erro", "Desculpe, não é possível validar presença com foto de outra foto");
                
                throw new ResponseException("Erro", "Erro ao carregar arquivos para o reconhecimento");
            } finally {
                for(int i = 0; i < 3; i++) {
                    File.Delete(ImagesPathWithPrefix + "tmpImage" + i + ".jpg");
                }
                File.Delete(ImagesPathWithPrefix + "BaseImage.jpg");
            }
        }
        
        private void ThreadAdjsutImage(object param) {
            ThreadParam threadParam = (ThreadParam)param;

            MemoryStream memoryStream = new MemoryStream(threadParam.Photo);
            Image image = Bitmap.FromStream(memoryStream);
            image = ToPortrait(image);
            if(threadParam.Adjust > 0)
                image = AdjustBrightness(image, (int)(threadParam.Adjust / 3f));

            if(threadParam.Index == 1) {
                SaveImage(image, ImagesPathWithPrefix + "tmpImage" + threadParam.Index);
            } else if(threadParam.Index == 2) {
                SaveImage(image, ImagesPathWithPrefix + "tmpImage" + threadParam.Index);
            }
        }

        private Image ToPortrait(Image image) {
            Image imageAux = image;

            if(imageAux.Height < imageAux.Width)
                imageAux.RotateFlip(RotateFlipType.Rotate270FlipNone);

            return imageAux;
        }
        private bool SaveImage(Image image, string fileName) {
            try {
                try {
                    File.Delete(fileName + ".jpg");
                } catch { }
                image.Save(fileName + ".jpg", ImageFormat.Jpeg);
                return true;
            } catch {
                return false;
            }
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

        public FacenetResponseInformations RecognizeFace(string baseImage, string img1, string img2, string img3) {
            FacenetHandler facenet = new FacenetHandler();

            FacenetRequestInformations requestInformations = new FacenetRequestInformations { baseImage = baseImage, img1 = img1, img2 = img2, img3 = img3 };

            WebRequest request = facenet.SendMessage("recognizeFaces", "POST", requestInformations);

            FacenetResponseInformations facenetResponse = facenet.GetWebResponse(request);

            return facenetResponse;
        }
    }
}