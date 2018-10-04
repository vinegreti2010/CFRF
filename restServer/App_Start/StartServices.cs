﻿using Database;
using System;
using System.Diagnostics;
using System.IO;

namespace App_Start {
    public class StartServices {
        Process facenetProcess;
        public void StartDatabase() {
            DatabaseHandler database = Singleton<DatabaseHandler>.Instance();
            database.OpenConnection();
        }

        public void StartFacenetServer() {
            /********************************************************************************
            *                                Local Debug                                    *
            ********************************************************************************/
            //string imagesPath = "C:/Users/Administrador/Desktop/ImagesTcc/";
            string faceNetExec = "\"C:/Users/Administrador/Documents/Visual Studio 2017/Projects/restServer/restServer/restServer/BackEnd/Python/Facenet/pythonCodes/FacenetRestServer.py\"";
            string pythonPath = "C:/Python36/python.exe";

            /********************************************************************************
            *                               Azure Server                                    *
            ********************************************************************************/
            /*//string imagesPath = "D:/home/site/wwwroot/Images/";
            string faceNetExec = "D:/home/site/wwwroot/BackEnd/Python/Facenet/pythonCodes/FacenetRestServer.py";
            string pythonPath = "D:/home/python364x64/python.exe";*/

            ProcessStartInfo start = new ProcessStartInfo {
                FileName = pythonPath,
                Arguments = faceNetExec,
                UseShellExecute = false,// Do not use OS shell
                CreateNoWindow = true, // We don't need new window
                RedirectStandardOutput = true,// Any output, generated by application will be redirected back
                RedirectStandardError = true // Any error in standard output will be redirected back (for example exceptions)
            };
            //Process process = null;
            facenetProcess = null;
            try {
                //process = Process.Start(start);
                facenetProcess = Process.Start(start);
                //using(StreamReader reader = facenetProcess.StandardOutput) {
                //string stderr = facenetProcess.StandardError.ReadToEnd(); // Here are the exceptions from our Python script
                //  string result = reader.ReadToEnd(); // Here is the result of StdOut(for example: print "test")
                //}
                //process.WaitForExit();
                facenetProcess.WaitForExit();
            } catch {
                //process.Close();
                facenetProcess.Close();
                throw new Exception("Erro ao abrir servidor facenet");
            }
        }

        public void Dispose() {
            facenetProcess.Kill();
            facenetProcess.Dispose();
        }
    }
}