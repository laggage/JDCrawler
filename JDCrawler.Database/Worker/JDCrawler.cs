using HtmlAgilityPack;
using JDCrawler.Core.Models;
using JDCrawler.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JDCrawler.Infrastructure.Worker
{
    public class JDCrawler
    {
        private const string SearchKey = "手机";
        public const string JDSearchBaseUrl = "https://search.jd.com/Search";
        public const string JDCommodityItemBaseUrl = "https://item.jd.com/{0}.html";

        private static MobileRepository _mobileRepository = new MobileRepository();

        public static IEnumerable<Mobile> SearchMobile(int pageStartIndex = 1, int pageCount = 1)
        {
            List<Mobile> mobiles = new List<Mobile>();
            for (int i = pageStartIndex; i < pageCount + 1; i++)
                mobiles.AddRange(GetMobileByPage(pageStartIndex));
            return mobiles;
        }

        public static void DisplayCommodities(IEnumerable<Commodity> commodities)
        {
            foreach (Commodity commodity in commodities)
            {
                Console.WriteLine("商品编号 : " + commodity.CommodityId);
                Console.WriteLine("商品名称 : " + commodity.Name);
                Console.WriteLine("商品描述 : " + commodity.Description);
                Console.WriteLine("价格 : " + commodity.Price);
            }
        }

        public static void DisplayMobiles(IEnumerable<Mobile> mobiles)
        {
            foreach (Mobile m in mobiles)
            {
                Console.WriteLine("Guid : " + m.Guid.ToString());
                Console.WriteLine("商品编号 : " + m.CommodityId);
                Console.WriteLine("商品名称 : " + m.Name);
                Console.WriteLine("商品描述 : " + m.Description);
                Console.WriteLine("商品产地 : " + m.Origin);
                Console.WriteLine("店铺Guid : " + m.Seller.Guid);
                Console.WriteLine("店铺 : " + m.Seller.Name);
                Console.WriteLine("拿到数据时间 : " + m.RecoredTime.ToString("yy-mm-dd HH-MM-ss"));
                Console.WriteLine("价格 : " + m.Price);
                Console.WriteLine("手机品牌 : " + m.Brand);
                Console.WriteLine("手机型号: " + m.ModelNumber);
                Console.WriteLine("CPU型号 : " + m.CPUModelNumber);
                Console.WriteLine("CPU核心数 : " + m.CPUCoreNumber);
                Console.WriteLine("手机ROM : " + m.ROM);
                Console.WriteLine("手机RAM : " + m.RAM);
                Console.WriteLine("上市时间 : " + m.MarketTime.ToString("yy年 MM月") + "\r\n\r\n");
            }
        }

        public static string SetUrlParameters(string url, Dictionary<string, string> parameters)
        {
            string result = url;

            if (parameters.Count > 0)
            {
                result += "?";
                foreach (KeyValuePair<string, string> keyValuePair in parameters)
                {
                    result = string.Format("{0}{1}={2}&", result, keyValuePair.Key,
                        keyValuePair.Value);
                }

                result = result.TrimEnd('&');
            }

            return result;
        }

        private static Mobile CreateMobile(HtmlNode itemNode)
        {
            MobileRepository rep = new MobileRepository();

            Mobile m = new Mobile()
            {
                Guid = Guid.NewGuid(),
                RecoredTime = DateTime.Now
            };
            m.CommodityId = itemNode.Attributes["data-sku"].Value;

            HtmlNode wrapNode =
                itemNode.SelectSingleNode("./div[@class='gl-i-wrap']");

            m.Description = wrapNode
                .SelectSingleNode("./div[@class='p-name p-name-type-2']/a/em")
                ?.InnerText;

            decimal.TryParse(
                wrapNode.SelectSingleNode(
                    "./div[@class='p-price']/strong/i")?.InnerText, out decimal price);
            m.Price = price;

            string shopName = wrapNode
                .SelectSingleNode("./div[@class='p-shop']//a[@class='curr-shop']")
                ?.Attributes["title"]?.Value;
            m.Seller = rep.GetShopByName(shopName) ?? new Shop
            {
                Guid = Guid.NewGuid(),
                Name = shopName
            };
            rep.Dispose();

            LoadMobileInfo(m);
            return m;
        }

        public static void LoadMobileInfo(Mobile m)
        {
            using (Stream s = CreateCommodityInfoRequest(m.CommodityId).GetResponse().GetResponseStream())
            {
                HtmlDocument doc = new HtmlDocument();
                doc.Load(s, Encoding.GetEncoding("GB2312"));
                HtmlNodeCollection simpleInfoNodes = doc.DocumentNode.SelectNodes(
                    "//ul[@class='parameter2 p-parameter-list']/li");
                HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(
                    "//div[@class='Ptable-item']");

                //商品介绍 - 商品名称
                m.Name = simpleInfoNodes.FirstOrDefault(
                        n => n.InnerText.Contains("商品名称"))
                    ?.Attributes["title"].Value;
                //商品介绍 - 商品产地
                m.Origin = simpleInfoNodes.FirstOrDefault(
                        n => n.InnerText.Contains("商品产地"))
                    ?.Attributes["title"].Value;

                //主体 - 品牌
                HtmlNode mainNode = nodes.FirstOrDefault(
                    node => node.SelectSingleNode("./h3").InnerText == "主体");
                m.Brand = mainNode?.SelectNodes(".//dl[@class='clearfix']")
                    .FirstOrDefault(n => n.SelectSingleNode("./dt").InnerText == "品牌")
                    ?.SelectSingleNode("./dd[last()]")
                    ?.InnerText;
                //主体 - 型号
                m.ModelNumber = mainNode.SelectNodes(".//dl[@class='clearfix']")
                    ?.FirstOrDefault(n => n.SelectSingleNode("./dt").InnerText == "型号")
                    ?.SelectSingleNode("./dd[last()]")
                    ?.InnerText;
                //主体 - 上市年份
                int.TryParse(mainNode.SelectNodes(".//dl[@class='clearfix']")
                    ?.FirstOrDefault(n => n.SelectSingleNode("./dt").InnerText == "上市年份")
                    ?.SelectSingleNode("./dd[last()]")
                    ?.InnerText.Replace("年", ""), out int year);
                //主体 - 上市月份
                int.TryParse(mainNode.SelectNodes(".//dl[@class='clearfix']")
                    ?.FirstOrDefault(n => n.SelectSingleNode("./dt").InnerText == "上市月份")
                    ?.SelectSingleNode("./dd[last()]")
                    ?.InnerText.Replace("月", ""), out int month);
                m.MarketTime = new DateTime(year, month <= 0 || month > 12 ? 1 : month, 1);

                //存储 - ROM
                HtmlNode storageInfoNode = nodes.FirstOrDefault(
                    n => n.SelectSingleNode("./h3").InnerText == "存储");
                int.TryParse(storageInfoNode.SelectNodes(".//dl[@class='clearfix']")
                    .FirstOrDefault(n => n.SelectSingleNode("./dt").InnerText == "ROM")
                    ?.SelectSingleNode("./dd[last()]").InnerText.Replace("GB", ""), out int rom);
                int.TryParse(storageInfoNode.SelectNodes(".//dl[@class='clearfix']")
                    .FirstOrDefault(n => n.SelectSingleNode("./dt").InnerText == "RAM")
                    ?.SelectSingleNode("./dd[last()]").InnerText.Replace("GB", ""), out int ram);
                m.ROM = rom;
                m.RAM = ram;

                // 主芯片 - CPU型号
                HtmlNode cpuNode = nodes.FirstOrDefault(
                    n => n.SelectSingleNode("./h3").InnerText == "主芯片");
                m.CPUModelNumber = cpuNode?.SelectNodes(".//dl[@class='clearfix']")
                       ?.FirstOrDefault(
                           n => n.SelectSingleNode("./dt").InnerText == "CPU型号")
                       ?.SelectSingleNode("./dd[last()]").InnerText;
                // 主芯片 - CPU核心数
                m.CPUCoreNumber = cpuNode?.SelectNodes(".//dl[@class='clearfix']")
                    ?.FirstOrDefault(
                        n => n.SelectSingleNode("./dt").InnerText == "CPU核数")
                    ?.SelectSingleNode("./dd[last()]").InnerText;

                s.Close();
            }
        }

        /// <summary>
        /// 获取某一页的手机信息
        /// </summary>
        /// <param name="pageIndex">这个参数必须是基数</param>
        /// <returns></returns>
        private static IEnumerable<Mobile> GetMobileByPage(int pageIndex = 1)
        {
            int n = 2 * pageIndex - 1;

            Stream[] stream =
            {
                CreateSearchRequest(SearchKey, 100 * 1000, n).GetResponse().GetResponseStream(),
                CreateSearchRequest(SearchKey, 100 * 1000, n + 1, true).GetResponse().GetResponseStream()
            };

            HtmlDocument[] doc = new HtmlDocument[2]
            {
                new HtmlDocument(),
                new HtmlDocument()
            };
            for (int i = 0; i < doc.Length; i++)
                doc[i].Load(stream[i], Encoding.UTF8);


            HtmlNodeCollection[] commoditiesLists = new HtmlNodeCollection[]
            {
                doc[0].DocumentNode.SelectNodes("//div[@id='J_goodsList']//li[@class='gl-item']"),
                doc[1].DocumentNode.SelectNodes("//div[@id='J_goodsList']//li[@class='gl-item']")
            };

            List<Mobile>[] mobileLists = new List<Mobile>[]
            {
                new List<Mobile>(),
                new List<Mobile>(),
                new List<Mobile>()
            };
            Parallel.For(0, commoditiesLists.Length, i =>
             {
                 mobileLists[i].AddRange(commoditiesLists[i].Select(node => CreateMobile(node)).ToList());
             });

            mobileLists[2].AddRange(mobileLists[0]);
            mobileLists[2].AddRange(mobileLists[1]);
            //foreach (HtmlNodeCollection nodeCollection in commoditiesLists)
            //{
            //    mobileLists[2].AddRange(nodeCollection.Select(
            //        node => CreateMobile(node)));
            //}

            foreach (Stream s in stream)
            {
                s.Close();
                s.Dispose();
            }

            return mobileLists[2];
        }

        private static HttpWebRequest CreateCommodityInfoRequest(string itemId)
        {
            string reqUrl = string.Format(JDCommodityItemBaseUrl, itemId);
            HttpWebRequest req = WebRequest.Create(reqUrl) as HttpWebRequest;
            req.Timeout = 30 * 1000;
            req.Method = "GET";
            req.Accept =
                "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3";
            req.UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Safari/537.36";

            return req;
        }

        private static HttpWebRequest CreateSearchRequest(string searchKey, int timeout,
            int pageIndex, bool scrolling = false)
        {
            string requestUrl = SetUrlParameters(JDSearchBaseUrl, new Dictionary<string, string>
            {
                {"keyword", HttpUtility.UrlEncode(searchKey, Encoding.UTF8).ToUpper()},
                {"enc", "utf-8"},
                {"page","1" }
            });
            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
            if (request == null)
                throw new ApplicationException("无法创建连接请求");

            request.Method = "GET";
            request.Accept =
                "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3";
            request.UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Safari/537.36";

            if (scrolling)
            {
                request.Headers.Add("s", "48");
                request.Headers.Add("scrolling", "y");
            }
            request.Timeout = timeout;
            return request;
        }

    }
}
