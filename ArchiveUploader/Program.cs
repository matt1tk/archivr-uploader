using System;
using System.Collections.Specialized;
using System.Threading;
using System.IO;
using System.Linq;
using System.Net;

namespace ArchiveUploader
{
    internal static class Program
    {
        static void Main()
        {
            if (!File.Exists("config.txt"))
            {
                Console.WriteLine("Enter the text file location: ");
                string filelocation = Console.ReadLine();
                Console.WriteLine("Enter your contact info, like email or reddit username (/u/username): ");
                string contactinfo = Console.ReadLine();
                Thread.Sleep(1000);
                File.WriteAllText("config.txt", filelocation + "|" + contactinfo);

            }
            string config = File.ReadLines("config.txt").First();
            string filepath = config.Split('|')[0];
            string contact = config.Split('|')[1];
            double lineCount = File.ReadLines(filepath).Count();
            double  current = 0;
            foreach (string line in File.ReadLines(filepath).Reverse())
            {
                string site = line.Split(' ')[0]; //site is first word
                string id = line.Split(' ')[1]; //id is second
                string url = "https://archivr.tk/submit.php"; //url to submit to
                using (var client = new WebClient())
                {
                    var values =
                        new NameValueCollection { { "site", site }, { "id", id }, { "contact", contact } }; //post request
                    client.UploadValues(new Uri(url), "POST", values);
                    client.Dispose();
                    current++;
                }
                Console.WriteLine("Uploaded " + line + "  |  " + current +"/" + lineCount + " (" + Math.Round((current/lineCount)*100, 5) + "% done)" );
            }
        }
    }
}
