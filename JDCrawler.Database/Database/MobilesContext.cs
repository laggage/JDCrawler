using JDCrawler.Core.Models;
using MySql.Data.Entity;
using System.Data.Entity;

namespace JDCrawler.Infrastructure.Database
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class MobilesContext : DbContext
    {
        public DbSet<Mobile> Mobiles { get; set; }
        //public DbSet<CommodityType> CommodityTypes { get; set; }
        public DbSet<Shop> Shops { get; set; }
        //public DbSet<ShopMobileTable> ShopMobileTable { get; set; }
        public MobilesContext():base("MobilesContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mobile>().ToTable("Mobile");
            modelBuilder.Entity<Mobile>().HasKey(m => m.Guid);
            modelBuilder.Entity<Mobile>().Property(m => m.Description).HasMaxLength(200);

            modelBuilder.Entity<Shop>().ToTable("Shop");
            modelBuilder.Entity<Shop>().HasMany(s => s.Mobiles)
                .WithMany(m => m.Shops)
                .Map(m =>
                {
                    m.MapLeftKey("ShopName");
                    m.MapRightKey("MobileGuid");
                });
            modelBuilder.Entity<Shop>().HasKey(s => s.Name);

            //modelBuilder.Entity<ShopMobileTable>().ToTable("ShopMobiles");


            base.OnModelCreating(modelBuilder);
        }
    }
}
