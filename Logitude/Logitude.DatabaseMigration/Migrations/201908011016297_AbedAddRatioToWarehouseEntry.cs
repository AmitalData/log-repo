namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddRatioToWarehouseEntry : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WarehouseEntries", "Ratio", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WarehouseEntries", "Ratio");
        }
    }
}
