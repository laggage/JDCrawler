using System;
using System.Collections.Generic;

namespace JDCrawler.Core.Models
{
    /// <summary>
    /// 代表京东店铺
    /// </summary>
    public class Shop
    {
        public string Name { get; set; }

        public virtual ICollection<Mobile> Mobiles { get; set; }
    }

    public class ShopComparer : IEqualityComparer<Shop>
    {
        public bool Equals(Shop x, Shop y)
        {
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;
            
            return x.Name == y.Name;
        }

        public int GetHashCode(Shop obj)
        {
            return this.GetHashCode() ^ obj.Name.GetHashCode();
        }
    }
}
