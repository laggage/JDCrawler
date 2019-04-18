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
            mobiles.Select(m =>
            {
                Shop s = GetShopById(m.Seller.Guid);
                if(s != null)
                    m.Seller = s;
                return m;
            });
            _ctx.Mobiles.AddRange(mobiles);
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

        public Shop GetShopById(Guid id)
        {
            return _ctx.Shops.ToList().FirstOrDefault(s => s.Guid == id);
        }

        public Shop AddShopByName(string shopName, bool saveChanges = false)
        {
            Shop newShop = new Shop
            {
                Guid = Guid.NewGuid(),
                Name = shopName
            };
            _ctx.Shops.Add(newShop);
            _ctx.SaveChanges();
            return newShop;
        }

        public IEnumerable<Shop> GetShops()
        {
            return _ctx.Shops.ToList();
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }
    }
}
