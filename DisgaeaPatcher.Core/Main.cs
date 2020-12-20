using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using DisgaeaPatcher.Core.Disgaea;
using DisgaeaPatcher.Core.FileManipulation;
using Newtonsoft.Json;

namespace DisgaeaPatcher.Core
{
    public class Main
    {
        // URLs for download the patch and version.json
        private string versionCheckUrl = "https://tradusquare.es/disgaea/version.json";
        private string fileDownloadUrl = "https://tradusquare.es/disgaea/DisgaeaPatchTest.zip";

        // Core variables
        public bool Installed { get; set; }

        // Debug

        //private string gameDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
        //                         Path.DirectorySeparatorChar;
        
        // Release
        
        private string gameDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                                 Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".."
                                 + Path.DirectorySeparatorChar;


        private string errMsg = "El juego está dañado, reinstala de nuevo el juego y el parcheador.";
        private string temp;
        public bool Internet { get; }
        public bool GamePirated;
        private string steamDllMd5 = "1147b3abee03804b62f11080588dc950";
        public Version LocalVersion { get; set; }
        public Version OnlineVersion { get; set; }

        public Main()
        {
            Internet = CheckInternet();
            Installed = File.Exists($"{gameDir}version.json");
            if (Installed)
            {
                LocalVersion = JsonConvert.DeserializeObject<Version>(
                    File.ReadAllText($"{gameDir}version.json"));
                steamDllMd5 = LocalVersion.Md5[3];
            }
                
        }

        public void OpenGame(int language)
        {
            switch (language)
            {
                case 0: // English
                    Process.Start($"{gameDir}dis1_st_en.exe");
                    break;
                case 1: // Spanish
                    Process.Start($"{gameDir}dis1_st_es.exe");
                    break;
                default:
                    Process.Start($"{gameDir}dis1_st_es.exe");
                    break;
            }
            
            System.Environment.Exit(0);
        }

        public (bool, string) CheckGame()
        {
            try
            {
                if (!File.Exists($"{gameDir}SUBDATA.DAT") || !File.Exists($"{gameDir}DATA.DAT") || !File.Exists($"{gameDir}dis1_st_en.exe") || !File.Exists($"{gameDir}steam_api.dll"))
                    return (false, $"{errMsg}\nERROR CODE: FILES_NOT_FOUND.");

                var dllMd5 = Md5.CalculateMd5($"{gameDir}steam_api.dll");

                if (Installed)
                {
                    if (dllMd5 == steamDllMd5 && !File.Exists($"{gameDir}SmartSteamEmu.dll"))
                        return (true, string.Empty);

                    GamePirated = true;
                    return (true, "ATENCIÓN: Se ha detectado una copia ilegítima de Disgaea PC.\n" +
                                  "Es posible que el parche no funcione correctamente. " +
                                  "Hypertraducciones no apoya la piratería, por lo que no podrá ofrecerte soporte.");
                }

                if (!Internet)
                    return (false,
                        "No tienes una conexión a internet. Es necesario tener internet\npara instalar el parche.");

                LocalVersion = GetVersionJson();

                var dataMd5 = Md5.CalculateMd5($"{gameDir}DATA.DAT");
                var subdataMd5 = Md5.CalculateMd5($"{gameDir}SUBDATA.DAT");
                var elfMd5 = Md5.CalculateMd5($"{gameDir}dis1_st_en.exe");


                if (LocalVersion.Md5[0] != elfMd5)
                    return (false, $"{errMsg}\nERROR CODE: ELF_MD5_MISMATCH.");

                if (LocalVersion.Md5[1] != dataMd5)
                    return (false, $"{errMsg}\nERROR CODE: DATA_MD5_MISMATCH.");

                if (LocalVersion.Md5[2] != subdataMd5)
                    return (false, $"{errMsg}\nERROR CODE: SUBDATA_MD5_MISMATCH.");

                if (dllMd5 != steamDllMd5 || File.Exists($"{gameDir}SmartSteamEmu.dll"))
                {
                    GamePirated = true;
                    return (true, "ATENCIÓN: Se ha detectado una copia ilegítima de Disgaea PC.\n" +
                                  "Es posible que el parche no funcione correctamente. " +
                                  "Hypertraducciones no apoya la piratería, por lo que no podrá ofrecerte soporte.");
                }


                return (true, string.Empty);
            }
            catch (Exception e)
            {
                return (false, $"{errMsg}\n{e}");
            }
        }

        public bool CheckUpdate()
        {
            if (!Internet)
                return false;

            OnlineVersion = GetVersionJson();

            return OnlineVersion.Id != LocalVersion.Id;
        }

        public (bool, string) PatchGame()
        {
            try
            {
                FileManipulation.Internet.GetFile(fileDownloadUrl, "DisgaeaPatch.zip", temp);
                Directory.CreateDirectory($"{temp}{Path.DirectorySeparatorChar}Update");
                ZipFile.ExtractToDirectory($"{temp}{Path.DirectorySeparatorChar}DisgaeaPatch.zip", $"{temp}{Path.DirectorySeparatorChar}Update");
                var result = PatchDisgaeaFiles.Patch(gameDir, $"{temp}{Path.DirectorySeparatorChar}Update");
                
                Installed = result.Item1;
                
                if (Installed) 
                    File.Copy($"{temp}{Path.DirectorySeparatorChar}version.json", $"{gameDir}version.json", true);

                return result;
            }
            catch (Exception e)
            {
                return (false, $"Se ha producido un error al aplicar el parche.\n{e}");
            }
            
        }

        private Version GetVersionJson()
        {
            GenerateTempFolder();
            if (Internet)
            {
                FileManipulation.Internet.GetFile(versionCheckUrl, "version.json", temp);
                return JsonConvert.DeserializeObject<Version>(
                    File.ReadAllText($"{temp}{Path.DirectorySeparatorChar}version.json"));
            }
            return null;
        }

        /// <summary>
        /// Genera una carpeta temporal en %temp%
        /// </summary>
        private void GenerateTempFolder()
        {
            temp = Path.GetTempPath() + Path.DirectorySeparatorChar + Path.GetRandomFileName();
            Directory.CreateDirectory(temp);
        }

        /// <summary>
        /// Comprueba si hay internet
        /// </summary>
        private bool CheckInternet()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("https://tradusquare.es/"))
                    return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
