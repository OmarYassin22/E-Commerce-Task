namespace E_Commerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameTotoalPriceColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "TotoalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Orders", "TotalPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "TotalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Orders", "TotoalPrice");
        }
    }
}
