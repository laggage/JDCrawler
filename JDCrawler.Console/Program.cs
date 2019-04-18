using JDCrawler.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crawler = JDCrawler.Infrastructure.Worker.JDCrawler;

namespace JDCrawler.Console
{
    class Program
    {
        static void Main(string[] args)
        {
           List<Mobile> mobiles = Crawler.SearchMobile(1,5).ToList();
           System.Console.WriteLine("共找到:{0}数据", mobiles.Count);
           Crawler.DisplayMobiles(mobiles);

            System.Console.ReadKey();
        }
    }
}
