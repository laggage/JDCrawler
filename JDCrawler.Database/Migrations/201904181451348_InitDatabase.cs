namespace JDCrawler.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Mobile",
                c => new
                    {
                        Guid = c.Guid(nullable: false),
                        Brand = c.String(unicode: false),
                        ModelNumber = c.String(unicode: false),
                        CPUModelNumber = c.String(unicode: false),
                        CPUCoreNumber = c.String(unicode: false),
                        BatteryCapacity = c.Int(nullable: false),
                        ROM = c.Int(nullable: false),
                        RAM = c.Int(nullable: false),
                        MarketTime = c.DateTime(nullable: false, precision: 0),
                        CommodityId = c.String(unicode: false),
                        Name = c.String(unicode: false),
                        Description = c.String(maxLength: 200, storeType: "nvarchar"),
                        Origin = c.String(unicode: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RecoredTime = c.DateTime(nullable: false, precision: 0),
                        SellerGuid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Guid)                
                .ForeignKey("Shop", t => t.SellerGuid, cascadeDelete: true)
                .Index(t => t.SellerGuid);
            
            CreateTable(
                "Shop",
                c => new
                    {
                        Guid = c.Guid(nullable: false),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Guid)                ;
            
        }
        
        public override void Down()
        {
            DropForeignKey("Mobile", "SellerGuid", "Shop");
            DropIndex("Mobile", new[] { "SellerGuid" });
            DropTable("Shop");
            DropTable("Mobile");
        }
    }
}
