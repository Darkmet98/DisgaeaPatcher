using System;
using System.Net;

namespace DisgaeaPatcher.Core.FileManipulation
{
    public class Internet
    {
        public static void GetFile(string url, string name, string Folder)
        {
            var random = new Random();
            url += $"?random={random.Next()}";
            using (WebClient client = new WebClient()) client.DownloadFile(url, @Folder + "/" + name);
        }
    }
}
