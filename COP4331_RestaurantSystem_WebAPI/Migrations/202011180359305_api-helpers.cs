namespace COP4331_RestaurantSystem_WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class apihelpers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderItems", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.OrderItems", "MenuItemID", "dbo.MenuItems");
            DropTable("dbo.OrderItems");
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MenuItemID = c.Int(nullable: false),
                        OrderID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.MenuItems", t => t.MenuItemID, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true);
            
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItems", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.OrderItems", "MenuItemID", "dbo.MenuItems");
            DropTable("dbo.OrderItems");
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        OrderID = c.Int(nullable: false),
                        MenuItemID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OrderID, t.MenuItemID });
            
            
            AddForeignKey("dbo.OrderItems", "MenuItemID", "dbo.MenuItems", "ID", cascadeDelete: true);
            AddForeignKey("dbo.OrderItems", "OrderID", "dbo.Orders", "ID", cascadeDelete: true);
        }
    }
}
