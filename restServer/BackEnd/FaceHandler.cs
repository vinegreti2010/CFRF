using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Threading;
using Informations;
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
        List<string> ListOfFaces;
        int AdjustBright;

        private readonly object insertLock = new object(); 

        public Face(string studentCode, byte[] photo) {
            ListOfFaces = new List<string>();
            MemoryStream memoryStream = new MemoryStream(photo);
            Image image = Bitmap.FromStream(memoryStream);
            
            AdjustBright = 130 - MedBrightImage((Bitmap)image);
            if(AdjustBright > 0)
                image = AdjustBrightness(image, (int)(AdjustBright / 3f));

            MemoryStream m2 = new MemoryStream();
            image.Save(m2, ImageFormat.Jpeg);
            ListOfFaces.Add(Convert.ToBase64String(memoryStream.ToArray()));
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

            t1.Join();
            t2.Join();

            float distance = -10, time = -1;

            try {
                FacenetResponseInformations facenetResponse = RecognizeFace(Convert.ToBase64String(baseImage), ListOfFaces[0], ListOfFaces[1], ListOfFaces[2]);

                distance = facenetResponse.distance;
                time = facenetResponse.time;

                if(distance < 0.0f)
                    throw new ResponseException("Erro", "Erro inesperado, favor entrar em contato com o supoerte");

                return facenetResponse;
            } catch(ResponseException e) {
                if(distance == -1f)
                    throw new ResponseException("Erro", "Não foi possível detectar sua face, por favor tente novamente");

                if(distance == -2f)
                    throw new ResponseException("Erro", "Desculpe, não é possível validar presença com foto de outra foto");

                throw e;
            } catch { 
                throw new ResponseException("Erro", "Erro ao carregar arquivos para o reconhecimento");
            } finally {
                ListOfFaces.Clear();
            }
        }
        
        private void ThreadAdjsutImage(object param) {
            ThreadParam threadParam = (ThreadParam)param;

            MemoryStream memoryStream = new MemoryStream(threadParam.Photo);
            Image image = Bitmap.FromStream(memoryStream);
            if(threadParam.Adjust > 0)
                image = AdjustBrightness(image, (int)(threadParam.Adjust / 3f));

            MemoryStream memoryStreamImageAdjusted = new MemoryStream();
            image.Save(memoryStreamImageAdjusted, ImageFormat.Jpeg);
            lock(insertLock) {
                ListOfFaces.Add(Convert.ToBase64String(memoryStream.ToArray()));
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