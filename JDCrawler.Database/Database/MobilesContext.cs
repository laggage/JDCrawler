using JDCrawler.Core.Models;
using MySql.Data.MySqlClient;
using System.Data.Entity;

namespace JDCrawler.Infrastructure.Database
{
    //[DbConfigurationType(typeof(MySqlConfiguration))]
    public class MobilesContext : DbContext
    {
        private static MobilesContext _instance;
        public static MobilesContext Instance
        {
            get
            {
                if (_instance == null) _instance = new MobilesContext();
                return _instance;
            }
        }

        public DbSet<Mobile> Mobiles { get; set; }
        public DbSet<CommodityType> CommodityTypes { get; set; }
        public DbSet<Shop> Shops { get; set; }

        public MobilesContext():base("MobilesContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Mobile>().ToTable("Entity");
            modelBuilder.Entity<Mobile>().HasKey(m => m.Guid);
            modelBuilder.Entity<Mobile>().Property(m => m.Description).HasMaxLength(200);

            modelBuilder.Entity<CommodityType>().ToTable("CommodityType");
            modelBuilder.Entity<CommodityType>().HasKey(c => c.Guid);

            modelBuilder.Entity<Shop>().ToTable("Shop");
            modelBuilder.Entity<Shop>().HasKey(s => s.Guid);
        }
    }
}
