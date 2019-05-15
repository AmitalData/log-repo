namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingRetriesToScheduler_Rabaia : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.DWObjectFields", "IsCustom", c => c.Boolean(nullable: false));
            AddColumn("dbo.TasksScheduler", "Retries", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TasksScheduler", "Retries");
            //DropColumn("dbo.DWObjectFields", "IsCustom");
        }
    }
}
