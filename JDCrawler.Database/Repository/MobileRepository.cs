using JDCrawler.Core.Models;
using JDCrawler.Infrastructure.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JDCrawler.Infrastructure.Repository
{
    public class MobileRepository:IDisposable
    {
        private MobilesContext _ctx;
       
        public MobileRepository()
        {
            _ctx = new MobilesContext();
        }

        public void AddMobile(Mobile newMobile)
        {
            if (newMobile == null) throw new ArgumentNullException(nameof(newMobile));
            _ctx.Mobiles.Add(newMobile);
        }

        public void AddMobiles(IEnumerable<Mobile> mobiles)
        {
            List<Shop> shops = mobiles.Select(m => m.Shops)
                .SelectMany(s => s.ToList())
                .ToList();
            List<Shop> originShops = _ctx.Shops.ToList();
            shops = shops.Distinct(new ShopComparer()).ToList();

            mobiles = mobiles.Select(m =>
            {
                m.Shops = m.Shops?.Select(s =>
                {
                    Shop orginShop = originShops.FirstOrDefault(ss => ss.Name == s.Name);
                    if (orginShop == null)   //orginShop can't find in database
                        orginShop = shops.FirstOrDefault(ss => ss.Name == s.Name);
                    else
                        shops.Remove(s);
                        
                    return orginShop;
                }).ToList()??m.Shops;
                return m;
            }).ToList();

            _ctx.Mobiles.AddRange(mobiles);
            IEnumerable<Shop> sss = _ctx.Shops.Local.ToList();
            IEnumerable<Mobile> mmm = _ctx.Mobiles.Local.ToList();
            
            _ctx.SaveChanges();
        }

        public Mobile GetMobileById(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
            return _ctx.Mobiles.FirstOrDefault(m => m.Equals(id));
        }

        public Shop GetShopByName(string shopName)
        {
            try
            {
                return _ctx.Shops.ToList().FirstOrDefault(s => s.Name == shopName);
            }
            catch(Exception)
            {
                return null;
            }
        }

        //public void AddShop(Shop shop)
        //{
        //    _ctx.Shops.Add(shop);
        //}

        //public Shop AddShopByName(string shopName, bool saveChanges = false)
        //{
        //    Shop newShop = new Shop
        //    {
        //        Name = shopName
        //    };
        //    _ctx.Shops.Add(newShop);
        //    if(saveChanges == true)
        //        _ctx.SaveChanges();
        //    return newShop;
        //}

        //public IEnumerable<Shop> GetShops()
        //{
        //    return _ctx.Shops.ToList();
        //}

        public void Dispose()
        {
            _ctx.Dispose();
        }
    }
}
