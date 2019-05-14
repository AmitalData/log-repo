namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommodityNumberToWarehouseEntry_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WarehouseEntryPackages", "CommodityNumber", c => c.String(maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {           
            DropColumn("dbo.WarehouseEntryPackages", "CommodityNumber");            
        }
    }
}
