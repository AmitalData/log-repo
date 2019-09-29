namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbeddeleteDWFieldsFromSetting : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Settings", "IsFullBuildDWRunning");
            DropColumn("dbo.Settings", "IsIncrementalDWRunning");
            DropColumn("dbo.Settings", "DWNextRunTime");
            DropColumn("dbo.Settings", "LastIncrementalDWUpdateDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Settings", "LastIncrementalDWUpdateDate", c => c.DateTime());
            AddColumn("dbo.Settings", "DWNextRunTime", c => c.DateTime());
            AddColumn("dbo.Settings", "IsIncrementalDWRunning", c => c.Boolean(nullable: false));
            AddColumn("dbo.Settings", "IsFullBuildDWRunning", c => c.Boolean(nullable: false));
        }
    }
}
