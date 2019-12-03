namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedByPartner_Fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cards", "CreatedByPartner", c => c.String(maxLength: 25, unicode: false));
            AddColumn("dbo.ARInvoices", "CreatedByPartner", c => c.String(maxLength: 25, unicode: false));
            AddColumn("dbo.ARPayments", "CreatedByPartner", c => c.String(maxLength: 25, unicode: false));
            AddColumn("dbo.Shipments", "CreatedByPartner", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipments", "CreatedByPartner");
            DropColumn("dbo.ARPayments", "CreatedByPartner");
            DropColumn("dbo.ARInvoices", "CreatedByPartner");
            DropColumn("dbo.Cards", "CreatedByPartner");
        }
    }
}
