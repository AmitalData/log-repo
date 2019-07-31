namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsPaymentChequesActivatedFullAccountingSettingMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FullAccountingSettings", "IsPaymentChequesActivated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FullAccountingSettings", "IsPaymentChequesActivated");
        }
    }
}
