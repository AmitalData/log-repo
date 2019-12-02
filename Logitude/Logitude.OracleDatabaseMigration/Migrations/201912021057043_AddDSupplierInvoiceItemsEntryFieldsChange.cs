namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDSupplierInvoiceItemsEntryFieldsChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Customs.ConsignmentPackDangers", new[] { "DeclarationId", "ConsignmentNumber", "LineNumber" }, "Customs.ConsignmentPackages");
            DropForeignKey("Customs.ConsignmentPackDangers", "DangerousGoodsPackingReqCode", "Customs.DangerousGoodsPackingReqs");
            DropForeignKey("Customs.ConsignmentPackDangers", "UNCode", "Customs.HazardousSubstances");
            DropForeignKey("Customs.DecDangersContacts", "CompanyCommTypeCode", "Customs.CommunicationTypes");
            DropForeignKey("Customs.DecDangersContacts", "ContactCommTypeCode", "Customs.CommunicationTypes");
            DropForeignKey("Customs.DecDangersContacts", "DeclarationId", "Customs.Declarations");
            DropForeignKey("dbo.SchedulerLogs", "HistoryId", "dbo.TaskSchedulerHistory");
            DropIndex("Customs.ConsignmentPackDangers", new[] { "DeclarationId", "ConsignmentNumber", "LineNumber" });
            DropIndex("Customs.ConsignmentPackDangers", new[] { "UNCode" });
            DropIndex("Customs.ConsignmentPackDangers", new[] { "DangerousGoodsPackingReqCode" });
            DropIndex("Customs.DecDangersContacts", new[] { "DeclarationId" });
            DropIndex("Customs.DecDangersContacts", new[] { "CompanyCommTypeCode" });
            DropIndex("Customs.DecDangersContacts", new[] { "ContactCommTypeCode" });
            DropIndex("dbo.SchedulerLogs", new[] { "HistoryId" });
            AlterColumn("Customs.SupplierInvoiceItems", "MarksAndNumbers", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("Customs.SupplierInvoiceItems", "Weight", c => c.Decimal(precision: 15, scale: 3));
            DropColumn("Customs.Declarations", "IsPaymentProtested");
            DropColumn("Customs.CourierMasters", "IsReadyForInvoice");
            DropTable("Customs.ConsignmentPackDangers");
            DropTable("Customs.HazardousSubstances");
            DropTable("Customs.DecDangersContacts");
            DropTable("dbo.SchedulerLogs");
            DropTable("dbo.SchedulerProcedure");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SchedulerProcedure",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 100, unicode: false),
                        Name = c.String(maxLength: 100, unicode: false),
                        SearchFields = c.String(maxLength: 1000, unicode: false),
                        Description = c.String(maxLength: 1000, unicode: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.SchedulerLogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false, precision: 7),
                        Log = c.String(unicode: false),
                        HistoryId = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Customs.DecDangersContacts",
                c => new
                    {
                        DeclarationId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CompanyName = c.String(nullable: false, maxLength: 70),
                        CompanyCommNumber = c.String(nullable: false, maxLength: 50, unicode: false),
                        CompanyCommTypeCode = c.String(nullable: false, maxLength: 2, unicode: false),
                        ContactName = c.String(nullable: false, maxLength: 70),
                        ContactCommNumber = c.String(nullable: false, maxLength: 50, unicode: false),
                        ContactCommTypeCode = c.String(nullable: false, maxLength: 2, unicode: false),
                        ContactId = c.String(maxLength: 5, unicode: false),
                    })
                .PrimaryKey(t => t.DeclarationId);
            
            CreateTable(
                "Customs.HazardousSubstances",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 4, unicode: false),
                        EnglishName = c.String(maxLength: 300, unicode: false),
                        SearchFields = c.String(),
                        LocalName = c.String(maxLength: 300),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "Customs.ConsignmentPackDangers",
                c => new
                    {
                        DeclarationId = c.String(nullable: false, maxLength: 15, unicode: false),
                        ConsignmentNumber = c.Int(nullable: false),
                        LineNumber = c.Int(nullable: false),
                        DangerousLineNo = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        UNCode = c.String(nullable: false, maxLength: 4, unicode: false),
                        DangerousGoodsPackingReqCode = c.String(maxLength: 3, unicode: false),
                        FlashpointTemperature = c.String(maxLength: 8, unicode: false),
                        StorageTemperature = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => new { t.DeclarationId, t.ConsignmentNumber, t.LineNumber, t.DangerousLineNo });
            
            AddColumn("Customs.CourierMasters", "IsReadyForInvoice", c => c.Boolean(nullable: false));
            AddColumn("Customs.Declarations", "IsPaymentProtested", c => c.Boolean(nullable: false));
            AlterColumn("Customs.SupplierInvoiceItems", "Weight", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("Customs.SupplierInvoiceItems", "MarksAndNumbers", c => c.String(maxLength: 512, unicode: false));
            CreateIndex("dbo.SchedulerLogs", "HistoryId");
            CreateIndex("Customs.DecDangersContacts", "ContactCommTypeCode");
            CreateIndex("Customs.DecDangersContacts", "CompanyCommTypeCode");
            CreateIndex("Customs.DecDangersContacts", "DeclarationId");
            CreateIndex("Customs.ConsignmentPackDangers", "DangerousGoodsPackingReqCode");
            CreateIndex("Customs.ConsignmentPackDangers", "UNCode");
            CreateIndex("Customs.ConsignmentPackDangers", new[] { "DeclarationId", "ConsignmentNumber", "LineNumber" });
            AddForeignKey("dbo.SchedulerLogs", "HistoryId", "dbo.TaskSchedulerHistory", "Id");
            AddForeignKey("Customs.DecDangersContacts", "DeclarationId", "Customs.Declarations", "Id");
            AddForeignKey("Customs.DecDangersContacts", "ContactCommTypeCode", "Customs.CommunicationTypes", "Code");
            AddForeignKey("Customs.DecDangersContacts", "CompanyCommTypeCode", "Customs.CommunicationTypes", "Code");
            AddForeignKey("Customs.ConsignmentPackDangers", "UNCode", "Customs.HazardousSubstances", "Code");
            AddForeignKey("Customs.ConsignmentPackDangers", "DangerousGoodsPackingReqCode", "Customs.DangerousGoodsPackingReqs", "Code");
            AddForeignKey("Customs.ConsignmentPackDangers", new[] { "DeclarationId", "ConsignmentNumber", "LineNumber" }, "Customs.ConsignmentPackages", new[] { "DeclarationId", "ConsignmentNumber", "LineNumber" });
        }
    }
}
