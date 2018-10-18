namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missing20180503 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.CustomsDocumentsDefinitions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        DocumentTypeCode = c.String(maxLength: 7, unicode: false),
                        TransportationTypeCode = c.String(maxLength: 1, unicode: false),
                        ProcessTypeCode = c.String(maxLength: 7, unicode: false),
                        CargoTypeCode = c.String(maxLength: 4, unicode: false),
                        Mandatory = c.Boolean(nullable: false),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                //.ForeignKey("Customs.CargoIdentifireTypes", t => t.CargoTypeCode)
                .ForeignKey("Customs.CustomDocumentTypes", t => t.DocumentTypeCode)
                .ForeignKey("Customs.CustomsTransportModes", t => t.TransportationTypeCode)
                .ForeignKey("Customs.GovernmentProcedureTypes", t => t.ProcessTypeCode)
                .Index(t => t.DocumentTypeCode)
                .Index(t => t.TransportationTypeCode)
                .Index(t => t.ProcessTypeCode)
                .Index(t => t.CargoTypeCode);
            
            //CreateTable(
            //    "Customs.UIMessages",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 10, unicode: false),
            //            LocalName = c.String(maxLength: 500),
            //            EnglishName = c.String(maxLength: 500, unicode: false),
            //            Inactive = c.Boolean(nullable: false),
            //            SearchFields = c.String(maxLength: 1500),
            //        })
            //    .PrimaryKey(t => t.Code);
            
            //CreateTable(
            //    "Customs.UIMessageAdditionals",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Code = c.String(nullable: false, maxLength: 10, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            Sort = c.Int(),
            //            SearchFields = c.String(maxLength: 1000),
            //        })
            //    .PrimaryKey(t => new { t.Id, t.Code })
            //    .ForeignKey("Customs.UIMessages", t => t.Code)
            //    .Index(t => t.Code);
            
            //AddColumn("Customs.SupplierInvoiceItems", "ClasifiedRemarks", c => c.String(maxLength: 1024));
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.UIMessageAdditionals", "Code", "Customs.UIMessages");
            DropForeignKey("Customs.CustomsDocumentsDefinitions", "ProcessTypeCode", "Customs.GovernmentProcedureTypes");
            DropForeignKey("Customs.CustomsDocumentsDefinitions", "TransportationTypeCode", "Customs.CustomsTransportModes");
            DropForeignKey("Customs.CustomsDocumentsDefinitions", "DocumentTypeCode", "Customs.CustomDocumentTypes");
            DropForeignKey("Customs.CustomsDocumentsDefinitions", "CargoTypeCode", "Customs.CargoIdentifireTypes");
            DropIndex("Customs.UIMessageAdditionals", new[] { "Code" });
            DropIndex("Customs.CustomsDocumentsDefinitions", new[] { "CargoTypeCode" });
            DropIndex("Customs.CustomsDocumentsDefinitions", new[] { "ProcessTypeCode" });
            DropIndex("Customs.CustomsDocumentsDefinitions", new[] { "TransportationTypeCode" });
            DropIndex("Customs.CustomsDocumentsDefinitions", new[] { "DocumentTypeCode" });
            DropColumn("Customs.SupplierInvoiceItems", "ClasifiedRemarks");
            DropTable("Customs.UIMessageAdditionals");
            DropTable("Customs.UIMessages");
            DropTable("Customs.CustomsDocumentsDefinitions");
        }
    }
}
