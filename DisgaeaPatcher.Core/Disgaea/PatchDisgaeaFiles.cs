using System;
using System.IO;
using Yarhl.IO;

namespace DisgaeaPatcher.Core.Disgaea
{
    class PatchDisgaeaFiles
    {
        public static (bool, string) Patch(string gameDir, string filesDir)
        {
            try
            {
                if (File.Exists($"{gameDir}DATA.esp"))
                    File.Delete($"{gameDir}DATA.esp");
                FileManipulation.Xdelta.Apply(new FileStream($"{gameDir}{Path.DirectorySeparatorChar}DATA.DAT", FileMode.Open),
                    File.ReadAllBytes($"{filesDir}{Path.DirectorySeparatorChar}Files{Path.DirectorySeparatorChar}DATA.xdelta"),
                    $"{gameDir}DATA.esp");

                if (File.Exists($"{gameDir}SUBDATA.esp"))
                    File.Delete($"{gameDir}SUBDATA.esp");
                FileManipulation.Xdelta.Apply(new FileStream($"{gameDir}{Path.DirectorySeparatorChar}SUBDATA.DAT", FileMode.Open),
                    File.ReadAllBytes($"{filesDir}{Path.DirectorySeparatorChar}Files{Path.DirectorySeparatorChar}SUBDATA.xdelta"),
                    $"{gameDir}SUBDATA.esp");

                if (File.Exists($"{gameDir}dis1_st_es.exe"))
                    File.Delete($"{gameDir}dis1_st_es.exe");
                FileManipulation.Xdelta.Apply(new FileStream($"{gameDir}{Path.DirectorySeparatorChar}dis1_st_en.exe", FileMode.Open),
                    File.ReadAllBytes($"{filesDir}{Path.DirectorySeparatorChar}Files{Path.DirectorySeparatorChar}dis1_st.xdelta"),
                    $"{gameDir}dis1_st_es.exe");

                PatchExe($"{gameDir}dis1_st_es.exe");
            }
            catch (Exception e)
            {
                return (false, $"Se ha producido un error al parchear los archivos.\n{e}");
            }

            return (true, "Se han parcheado los archivos.");
        }

        /// <summary>
        /// DISGAEA SPECIAL FUNCTION: Patch the exe for load the spanish files.
        /// </summary>
        private static void PatchExe(string exeDir)
        {
            using (var exe = DataStreamFactory.FromFile(exeDir, FileOpenMode.ReadWrite))
            {
                var writer = new DataWriter(exe);
                writer.Stream.Position = 0x127785;
                writer.Write("esp", false);
                writer.Stream.Position += 0xC;
                writer.Write("esp", false);
            }
        }
    }
}
