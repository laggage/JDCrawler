using JDCrawler.Core.Models;
using JDCrawler.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using Crawler = JDCrawler.Infrastructure.Worker.JDCrawler;

namespace JDCrawler.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Beigin...");
            List<Mobile> mobiles = Crawler.SearchMobile(60,1).ToList();

            Crawler.DisplayMobiles(mobiles);
            System.Console.WriteLine("数据爬取完成,共找到:{0}数据", mobiles.Count);

            System.Console.WriteLine("开始写入数据到MySql...");
            MobileRepository rep = new MobileRepository();
            rep.AddMobiles(mobiles);
            //clean up resource
            rep.Dispose();  
            GC.Collect();
            mobiles.Clear();
            mobiles = null;

            System.Console.WriteLine("数据写入成功.任意键退出.");
            System.Console.ReadKey();
        }
    }
}
