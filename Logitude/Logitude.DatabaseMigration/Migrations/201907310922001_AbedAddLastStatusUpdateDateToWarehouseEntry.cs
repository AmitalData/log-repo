namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddLastStatusUpdateDateToWarehouseEntry : DbMigration
    {
        public override void Up()
        {
     
            AddColumn("dbo.WarehouseEntries", "LastStatusUpdateDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WarehouseEntries", "LastStatusUpdateDate");
        }
    }
}
