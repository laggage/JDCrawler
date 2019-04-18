using System;

namespace JDCrawler.Core.Models
{
    /// <summary>
    /// 商品信息
    /// </summary>
    public abstract class Commodity
    {
        public Guid Guid { get; set; }
        public string CommodityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// 商品产地
        /// </summary>
        public string Origin { get; set; }

        public decimal Price { get; set; }
        /// <summary>
        /// 拿到商品信息的时间
        /// </summary>
        public DateTime RecoredTime { get; set; }

        public Guid SellerGuid { get; set; }
        public virtual Shop Seller { get; set; }
    }
}
