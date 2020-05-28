namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldAllowManualARPaymentNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountingSettings", "AllowManualARPaymentNumber", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountingSettings", "AllowManualARPaymentNumber");
        }
    }
}
