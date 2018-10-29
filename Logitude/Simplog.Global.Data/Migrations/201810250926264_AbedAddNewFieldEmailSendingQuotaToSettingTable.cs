namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddNewFieldEmailSendingQuotaToSettingTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "EmailSendingQuota", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "EmailSendingQuota");
        }
    }
}
