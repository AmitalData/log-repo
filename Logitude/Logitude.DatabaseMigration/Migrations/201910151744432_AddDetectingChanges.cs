namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDetectingChanges : DbMigration
    {
        public override void Up()
        {
            //DropIndex("dbo.JournalExternalReconciles", new[] { "ReconcileExternalPageLineId" });
            //CreateTable(
            //    "dbo.CustomsTransferHeaders",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            TransferNumber = c.String(nullable: false, maxLength: 20, unicode: false),
            //            TransferDate = c.DateTime(),
            //            FileName = c.String(nullable: false, maxLength: 40, unicode: false),
            //            CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            CustomsTransferTypeCode = c.String(nullable: false, maxLength: 4, unicode: false),
            //            SearchFields = c.String(maxLength: 1000),
            //            Notes = c.String(maxLength: 250),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Users", t => t.CreatedByUserId)
            //    .ForeignKey("dbo.CustomsTransferTypes", t => t.CustomsTransferTypeCode)
            //    .Index(t => t.CreatedByUserId)
            //    .Index(t => t.CustomsTransferTypeCode);
            
            //CreateTable(
            //    "dbo.CustomsTransferTypes",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 4, unicode: false),
            //            Name = c.String(nullable: false, maxLength: 40, unicode: false),
            //            SearchFields = c.String(maxLength: 1000),
            //        })
            //    .PrimaryKey(t => t.Code);
            
            //CreateTable(
            //    "dbo.CustomsTransferLines",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            CustomsTransferHeaderId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            SearchFields = c.String(maxLength: 1000),
            //            ShipmentId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            ShipmentNumber = c.String(maxLength: 20, unicode: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.CustomsTransferHeaders", t => t.CustomsTransferHeaderId)
            //    .Index(t => t.CustomsTransferHeaderId);
            
            //AddColumn("dbo.Shipments", "NotInvoicedReceivablesAmount", c => c.Double());
            //AlterColumn("dbo.LogBoxTenantSettings", "StockTypeCode", c => c.String());
            //AlterColumn("dbo.JournalExternalReconciles", "ReconcileExternalPageLineId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            //CreateIndex("dbo.JournalExternalReconciles", "ReconcileExternalPageLineId");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.CustomsTransferLines", "CustomsTransferHeaderId", "dbo.CustomsTransferHeaders");
            //DropForeignKey("dbo.CustomsTransferHeaders", "CustomsTransferTypeCode", "dbo.CustomsTransferTypes");
            //DropForeignKey("dbo.CustomsTransferHeaders", "CreatedByUserId", "dbo.Users");
            //DropIndex("dbo.CustomsTransferLines", new[] { "CustomsTransferHeaderId" });
            //DropIndex("dbo.CustomsTransferHeaders", new[] { "CustomsTransferTypeCode" });
            //DropIndex("dbo.CustomsTransferHeaders", new[] { "CreatedByUserId" });
            //DropIndex("dbo.JournalExternalReconciles", new[] { "ReconcileExternalPageLineId" });
            //AlterColumn("dbo.JournalExternalReconciles", "ReconcileExternalPageLineId", c => c.String(maxLength: 15, unicode: false));
            //AlterColumn("dbo.LogBoxTenantSettings", "StockTypeCode", c => c.String(maxLength: 15, unicode: false));
            //DropColumn("dbo.Shipments", "NotInvoicedReceivablesAmount");
            //DropTable("dbo.CustomsTransferLines");
            //DropTable("dbo.CustomsTransferTypes");
            //DropTable("dbo.CustomsTransferHeaders");
            //CreateIndex("dbo.JournalExternalReconciles", "ReconcileExternalPageLineId");
        }
    }
}
