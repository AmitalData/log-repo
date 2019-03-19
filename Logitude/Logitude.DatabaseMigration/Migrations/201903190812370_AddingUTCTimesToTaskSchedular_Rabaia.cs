namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingUTCTimesToTaskSchedular_Rabaia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TasksScheduler", "NextRunTimeUTC", c => c.DateTime());
            AddColumn("dbo.TasksScheduler", "LastRunTimeUTC", c => c.DateTime());
            AddColumn("dbo.TasksScheduler", "StartDateTimeUTC", c => c.DateTime());
            //AddColumn("dbo.TMBudgets", "Inactive", c => c.Boolean(nullable: false));
            //AddColumn("dbo.WarehouseEntries", "TotalVolumetricWeight", c => c.Decimal(nullable: false, precision: 18, scale: 3));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.WarehouseEntries", "TotalVolumetricWeight");
            //DropColumn("dbo.TMBudgets", "Inactive");
            DropColumn("dbo.TasksScheduler", "StartDateTimeUTC");
            DropColumn("dbo.TasksScheduler", "LastRunTimeUTC");
            DropColumn("dbo.TasksScheduler", "NextRunTimeUTC");
        }
    }
}
