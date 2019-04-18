namespace JDCrawler.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initilize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommodityType",
                c => new
                    {
                        Guid = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Guid);
            
            CreateTable(
                "dbo.Entity",
                c => new
                    {
                        Guid = c.Guid(nullable: false),
                        Brand = c.String(),
                        ModelNumber = c.String(),
                        CPUModelNumber = c.String(),
                        CPUCoreNumber = c.Int(nullable: false),
                        BatteryCapacity = c.Int(nullable: false),
                        ROM = c.Int(nullable: false),
                        RAM = c.Int(nullable: false),
                        MarketTime = c.DateTime(nullable: false),
                        CommodityId = c.String(),
                        Name = c.String(),
                        Description = c.String(maxLength: 200),
                        Origin = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RecoredTime = c.DateTime(nullable: false),
                        CommodityType_Guid = c.Guid(),
                        Seller_Guid = c.Guid(),
                    })
                .PrimaryKey(t => t.Guid)
                .ForeignKey("dbo.CommodityType", t => t.CommodityType_Guid)
                .ForeignKey("dbo.Shop", t => t.Seller_Guid)
                .Index(t => t.CommodityType_Guid)
                .Index(t => t.Seller_Guid);
            
            CreateTable(
                "dbo.Shop",
                c => new
                    {
                        Guid = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Guid);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Entity", "Seller_Guid", "dbo.Shop");
            DropForeignKey("dbo.Entity", "CommodityType_Guid", "dbo.CommodityType");
            DropIndex("dbo.Entity", new[] { "Seller_Guid" });
            DropIndex("dbo.Entity", new[] { "CommodityType_Guid" });
            DropTable("dbo.Shop");
            DropTable("dbo.Entity");
            DropTable("dbo.CommodityType");
        }
    }
}
