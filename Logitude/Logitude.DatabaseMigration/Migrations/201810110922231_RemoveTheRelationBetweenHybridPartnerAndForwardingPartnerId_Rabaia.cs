namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTheRelationBetweenHybridPartnerAndForwardingPartnerId_Rabaia : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.Shipments", "ForwarderPartnerId");
            //RenameColumn(table: "dbo.Shipments", name: "ForwardingPartnerId", newName: "ForwarderPartnerId");
            //RenameIndex(table: "dbo.Shipments", name: "IX_ForwardingPartnerId", newName: "IX_ForwarderPartnerId");
            //AddColumn("dbo.BankAccountLites", "VatNumber", c => c.String(maxLength: 20, unicode: false));
            //AddColumn("dbo.ARPayments", "MetodoPagoCode", c => c.String(maxLength: 3, unicode: false));
            //AddColumn("dbo.ShipmentPickUpDeliveryPackages", "OriginalShipmentPackageId", c => c.String(maxLength: 15, unicode: false));
            //AlterColumn("dbo.BankAccountLites", "BranchNumber", c => c.String(maxLength: 15, unicode: false));
            //CreateIndex("dbo.ARPayments", "MetodoPagoCode");
            //AddForeignKey("dbo.ARPayments", "MetodoPagoCode", "dbo.MetodoPagos", "Code");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.ARPayments", "MetodoPagoCode", "dbo.MetodoPagos");
            //DropIndex("dbo.ARPayments", new[] { "MetodoPagoCode" });
            //AlterColumn("dbo.BankAccountLites", "BranchNumber", c => c.String(nullable: false, maxLength: 15, unicode: false));
            //DropColumn("dbo.ShipmentPickUpDeliveryPackages", "OriginalShipmentPackageId");
            //DropColumn("dbo.ARPayments", "MetodoPagoCode");
            //DropColumn("dbo.BankAccountLites", "VatNumber");
            //RenameIndex(table: "dbo.Shipments", name: "IX_ForwarderPartnerId", newName: "IX_ForwardingPartnerId");
            //RenameColumn(table: "dbo.Shipments", name: "ForwarderPartnerId", newName: "ForwardingPartnerId");
            //AddColumn("dbo.Shipments", "ForwarderPartnerId", c => c.String(maxLength: 15, unicode: false));
        }
    }
}
