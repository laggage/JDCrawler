using System;
using System.Collections.Generic;

namespace JDCrawler.Core.Models
{
    /// <summary>
    /// 代表京东店铺
    /// </summary>
    public class Shop
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Mobile> Mobiles { get; set; }

        public bool Equals(Shop shop)
        {
            if (shop == null) return false;
            if (ReferenceEquals(shop, this)) return true;
            if (GetHashCode() != shop.GetHashCode()) return false;
            return Guid.Equals(shop.Guid);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) return false;

            return Equals((Shop)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode()^Guid.GetHashCode();
        }

        public static bool operator ==(Shop left, Shop right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Shop left, Shop right)
        {
            return !(left == right);
        }
    }
}
