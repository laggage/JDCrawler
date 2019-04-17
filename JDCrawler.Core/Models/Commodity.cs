namespace JDCrawler.Core.Models
{
    /// <summary>
    /// 商品信息
    /// </summary>
    public abstract class Commodity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// 商品产地
        /// </summary>
        public string Origin { get; set; }

        public decimal Price { get; set; }

        public virtual CommodityType CommodityType { get; set; }
        public virtual Shop Seller { get; set; }

    }
}
