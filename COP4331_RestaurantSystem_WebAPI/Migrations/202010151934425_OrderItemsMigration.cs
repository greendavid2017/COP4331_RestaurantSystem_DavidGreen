namespace COP4331_RestaurantSystem_WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderItemsMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MenuItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Category = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        Status = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        Submitted = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 256),
                        Password = c.String(nullable: false, maxLength: 512),
                        IsEmployee = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        OrderID = c.Int(nullable: false),
                        MenuItemID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OrderID, t.MenuItemID })
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.MenuItems", t => t.MenuItemID, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.MenuItemID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "UserID", "dbo.Users");
            DropForeignKey("dbo.OrderItems", "MenuItemID", "dbo.MenuItems");
            DropForeignKey("dbo.OrderItems", "OrderID", "dbo.Orders");
            DropIndex("dbo.OrderItems", new[] { "MenuItemID" });
            DropIndex("dbo.OrderItems", new[] { "OrderID" });
            DropIndex("dbo.Orders", new[] { "UserID" });
            DropTable("dbo.OrderItems");
            DropTable("dbo.Users");
            DropTable("dbo.Orders");
            DropTable("dbo.MenuItems");
        }
    }
}
