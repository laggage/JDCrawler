using JDCrawler.Core.Models;
using JDCrawler.Infrastructure.Database;
using System;
using System.Linq;

namespace JDCrawler.Infrastructure.Repository
{
    public class MobileRepository
    {
        private MobileRepository _instance;

        public MobileRepository Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MobileRepository();
                return _instance;
            }
        }

        private MobilesContext _ctx;


        private MobileRepository()
        {
            _ctx = MobilesContext.Instance;
        }

        public void AddMobile(Mobile newMobile)
        {
            if (newMobile == null) throw new ArgumentNullException(nameof(newMobile));
            _ctx.Mobiles.Add(newMobile);
            _ctx.SaveChanges();
        }

        public Mobile GetMobileById(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
            return _ctx.Mobiles.FirstOrDefault(m => m.Equals(id));
        }
    }
}
