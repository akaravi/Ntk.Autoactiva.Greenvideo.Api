using System;
using System.IO;

namespace Ntk.Autoactiva.Greenvideo.Core.Helper
{
    public static class ClientLogHelper
    {
        public static void ClientLogHelperSaveToFile(object text, string FileAddressLog)
        {
            try
            {
                var filiInfo = new FileInfo(FileAddressLog);
                if (filiInfo.Exists && filiInfo.Length > 1024 * 1024)
                    File.Delete(filiInfo.FullName);
                ////
                if (!string.IsNullOrEmpty(FileAddressLog))
                {
                    using (var sw = new StreamWriter(FileAddressLog, true))
                    {
                        sw.WriteLine("[" + DateTime.UtcNow.ToString() + "]" + text.ToString());
                        sw.Close();
                    }
                }
                ////
            }
            catch
            {

            }
        }
        public static void ClientLogHelperSaveToFolder(this object text, string FolderAddressLog, string Tag = "Log", bool printTime = false)
        {
            if (string.IsNullOrEmpty(FolderAddressLog))
                return;
            string fileName = "";
            if (string.IsNullOrEmpty(Tag))
                Tag = "Log";
            if (printTime)
                fileName = Tag + "_" + DateTime.Now.ToString("MMdd_HHmmss") + ".txt";
            else
                fileName = Tag + ".txt";
            try
            {
                if (!Directory.Exists(FolderAddressLog))
                {
                    Directory.CreateDirectory(FolderAddressLog);
                }
                ////
                if (Directory.Exists(FolderAddressLog))
                {
                    StreamWriter sw = new StreamWriter(FolderAddressLog + "\\" + fileName, true);
                    sw.WriteLine("[" + DateTime.Now.ToString() + "]" + text.JsonHelperSerializeObject());
                    sw.Close();
                }
                ////
            }
            catch
            {

            }
        }
    }
}
