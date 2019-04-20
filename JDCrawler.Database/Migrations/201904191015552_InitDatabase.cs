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
                    })
                .PrimaryKey(t => t.Guid)                ;
            
            CreateTable(
                "Shop",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Name)                ;
            
            CreateTable(
                "ShopMobiles",
                c => new
                    {
                        ShopName = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        MobileGuid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ShopName, t.MobileGuid })                
                .ForeignKey("Shop", t => t.ShopName, cascadeDelete: true)
                .ForeignKey("Mobile", t => t.MobileGuid, cascadeDelete: true)
                .Index(t => t.ShopName)
                .Index(t => t.MobileGuid);
            
        }
        
        public override void Down()
        {
            DropForeignKey("ShopMobiles", "MobileGuid", "Mobile");
            DropForeignKey("ShopMobiles", "ShopName", "Shop");
            DropIndex("ShopMobiles", new[] { "MobileGuid" });
            DropIndex("ShopMobiles", new[] { "ShopName" });
            DropTable("ShopMobiles");
            DropTable("Shop");
            DropTable("Mobile");
        }
    }
}
