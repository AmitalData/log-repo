namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeclarationReferantDataTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.DeclarationReferantDatas",
                c => new
                    {
                        DeclarationId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        OrderNumber = c.String(maxLength: 30, unicode: false),
                        VendorId = c.String(maxLength: 15, unicode: false),
                        ArrivalDate = c.DateTime(nullable: false, precision: 7),
                        EstimatedArrivalDate = c.DateTime(nullable: false, precision: 7),
                        Weight = c.Decimal(precision: 15, scale: 3),
                        ClassificationStatus = c.String(maxLength: 1, unicode: false),
                        ControllerStatus = c.String(maxLength: 1, unicode: false),
                        CollectionOfMoneyStatus = c.String(maxLength: 1, unicode: false),
                        FollowUpDate = c.DateTime(precision: 7),
                        IsExceptional = c.Boolean(nullable: false),
                        WithPaper = c.Boolean(nullable: false),
                        IsClosedForFollowUp = c.String(maxLength: 1, unicode: false),
                        IsClassificationRemarks = c.Boolean(nullable: false),
                        IsControllerRemarks = c.Boolean(nullable: false),
                        PreClassification = c.String(maxLength: 1, unicode: false),
                    })
                .PrimaryKey(t => t.DeclarationId)
                .ForeignKey("Customs.CustomsVendors", t => t.VendorId)
                .Index(t => t.VendorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.DeclarationReferantDatas", "VendorId", "Customs.CustomsVendors");
            DropIndex("Customs.DeclarationReferantDatas", new[] { "VendorId" });
            DropTable("Customs.DeclarationReferantDatas");
        }
    }
}
