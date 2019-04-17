using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JDCrawler.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            string htmlSavePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "File\\";
            string requsetUrl = @"https://search.jd.com/Search?keyword=%E6%89%8B%E6%9C%BA&enc=utf-8&wq=%E6%89%8B%E6%9C%BA&pvid=d29e806fe3b340479de64d967cd722f3";
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(
                new Uri(requsetUrl));
            webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3";
            webRequest.Method = "GET";
            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Safari/537.36";
            webRequest.Timeout = 30 * 1000;
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            Stream responseStream = webResponse.GetResponseStream();

            using (StreamReader reader = new StreamReader(responseStream, true))
            {
                WebClient client = new WebClient();
                client.Headers.Add(
                    HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
                client.Headers.Add(HttpRequestHeader.UserAgent,
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Safari/537.36");
                client.DownloadFile(
                    requsetUrl,htmlSavePath + "test.html");



                while (!reader.EndOfStream)
                    Console.WriteLine(reader.ReadLine());
            }
            webResponse.Close();
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.ReadKey();
        }
    }
}
