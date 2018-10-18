namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JornalMoreDATA2 : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.Shipments", "ForwarderPartnerId");
            //RenameColumn(table: "dbo.Shipments", name: "ForwardingPartnerId", newName: "ForwarderPartnerId");
            //RenameIndex(table: "dbo.Shipments", name: "IX_ForwardingPartnerId", newName: "IX_ForwarderPartnerId");
            //AddColumn("dbo.TaxReports", "NeedsRebulid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.TaxReports", "NeedsRebulid");
            //RenameIndex(table: "dbo.Shipments", name: "IX_ForwarderPartnerId", newName: "IX_ForwardingPartnerId");
            //RenameColumn(table: "dbo.Shipments", name: "ForwarderPartnerId", newName: "ForwardingPartnerId");
            //AddColumn("dbo.Shipments", "ForwarderPartnerId", c => c.String(maxLength: 15, unicode: false));
        }
    }
}
