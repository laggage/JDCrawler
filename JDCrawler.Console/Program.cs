using JDCrawler.Core.Models;
using JDCrawler.Infrastructure.Database;
using JDCrawler.Infrastructure.Repository;
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
           List<Mobile> mobiles = Crawler.SearchMobile(2,15).ToList();
           System.Console.WriteLine("共找到:{0}数据", mobiles.Count);
           Crawler.DisplayMobiles(mobiles);

           //foreach (Shop s in MobileRepository.Instance.GetShops())
           
           MobileRepository rep = new MobileRepository();
           rep.AddMobiles(mobiles);
            
          // UnitOfWork.Instance.SaveChanges();
           System.Console.WriteLine("数据保存成功!");
            

           System.Console.ReadKey();
        }
    }
}
