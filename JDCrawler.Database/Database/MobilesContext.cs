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

        public MobilesContext():base("MobilesContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mobile>().ToTable("Mobile");
            modelBuilder.Entity<Mobile>().HasKey(m => m.Guid);
            modelBuilder.Entity<Mobile>().Property(m => m.Description).HasMaxLength(200);
            //modelBuilder.Entity<Mobile>()
            //    .HasRequired(m => m.Seller)
            //    .WithMany(s => s.Mobiles)
            //    .Map(m => m.MapKey("SellerId"));

            modelBuilder.Entity<Shop>().ToTable("Shop");
            modelBuilder.Entity<Shop>().HasKey(s => s.Guid);

            base.OnModelCreating(modelBuilder);
        }
    }
}
