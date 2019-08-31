namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FullAccountingSettingsLogoMigration : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.INTTRABookingStatus", newName: "INTTRABookingStatuses");
            //RenameTable(name: "dbo.INTTRABookingTransStatus", newName: "INTTRABookingTransStatuses");
            //AddColumn("dbo.Users", "AdditionalPackagesOnly", c => c.Boolean(nullable: false));
            AddColumn("dbo.FullAccountingSettings", "PaymentChequesLogoId", c => c.String(maxLength: 15, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FullAccountingSettings", "PaymentChequesLogoId");
            DropColumn("dbo.Users", "AdditionalPackagesOnly");
            RenameTable(name: "dbo.INTTRABookingTransStatuses", newName: "INTTRABookingTransStatus");
            RenameTable(name: "dbo.INTTRABookingStatuses", newName: "INTTRABookingStatus");
        }
    }
}
