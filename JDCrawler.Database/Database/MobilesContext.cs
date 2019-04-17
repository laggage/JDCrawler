using JDCrawler.Core.Models;
using System.Data.Entity;

namespace JDCrawler.Infrastructure.Database
{
    public class MobilesContext : DbContext
    {
        private static MobilesContext _instance;
        public static MobilesContext Instance
        {
            get => _instance ?? new MobilesContext();
        }

        public DbSet<Mobile> Mobiles { get; set; }
        public DbSet<CommodityType> CommodityTypes { get; set; }
        public DbSet<Shop> Shops { get; set; }

        private MobilesContext()
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Mobile>().ToTable("Entity");
            modelBuilder.Entity<Mobile>().Property(m => m.Description).HasMaxLength(200);
            modelBuilder.Entity<CommodityType>().ToTable("CommodityType");
            modelBuilder.Entity<Shop>().ToTable("Shop");
        }
    }
}
