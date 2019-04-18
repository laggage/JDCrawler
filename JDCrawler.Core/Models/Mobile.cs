using System;

namespace JDCrawler.Core.Models
{
    /// <summary>
    /// 代表手机
    /// </summary>
    public class Mobile:Commodity
    {
        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public string ModelNumber { get; set; }
        /// <summary>
        /// CPU型号
        /// </summary>
        public string CPUModelNumber { get; set; }
        /// <summary>
        /// CPU核心数
        /// </summary>
        public string CPUCoreNumber { get; set; }
        /// <summary>
        /// 电池容量
        /// </summary>
        public int BatteryCapacity { get; set; }
        public int ROM { get; set; }
        public int RAM { get; set; }
        /// <summary>
        /// 上市时间
        /// </summary>
        public DateTime MarketTime { get; set; }

    }
}
