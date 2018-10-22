namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewField_BTE_Subject : DbMigration
    {
        public override void Up()
        {
            //DropIndex("dbo.INTTRASettings", new[] { "OutSettingsId" });
            AddColumn("dbo.BatchTaskExecutions", "Subject", c => c.String(maxLength: 200));
            //AlterColumn("dbo.INTTRASettings", "OutSettingsId", c => c.String(maxLength: 15, unicode: false));
            //CreateIndex("dbo.INTTRASettings", "OutSettingsId");
        }
        
        public override void Down()
        {
            //DropIndex("dbo.INTTRASettings", new[] { "OutSettingsId" });
            //AlterColumn("dbo.INTTRASettings", "OutSettingsId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            DropColumn("dbo.BatchTaskExecutions", "Subject");
            //CreateIndex("dbo.INTTRASettings", "OutSettingsId");
        }
    }
}
