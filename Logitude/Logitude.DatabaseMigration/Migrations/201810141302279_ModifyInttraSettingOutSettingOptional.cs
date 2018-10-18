namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyInttraSettingOutSettingOptional : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.INTTRASettings", new[] { "OutSettingsId" });
            AlterColumn("dbo.INTTRASettings", "OutSettingsId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.INTTRASettings", "OutSettingsId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.INTTRASettings", new[] { "OutSettingsId" });
            AlterColumn("dbo.INTTRASettings", "OutSettingsId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            CreateIndex("dbo.INTTRASettings", "OutSettingsId");
        }
    }
}
