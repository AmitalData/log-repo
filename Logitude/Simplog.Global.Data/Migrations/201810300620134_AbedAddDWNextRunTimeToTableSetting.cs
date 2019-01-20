namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddDWNextRunTimeToTableSetting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "DWNextRunTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "DWNextRunTime");
        }
    }
}
