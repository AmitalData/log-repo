namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newField_fullscreentext : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Customs.Declarations", "MamanStatusCode", "Customs.MamanStatuses");
            DropIndex("Customs.Declarations", new[] { "MamanStatusCode" });
            DropPrimaryKey("Customs.VendorCommissions");
            CreateTable(
                "dbo.ContinuousRequestTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        LocalName = c.String(),
                        SearchFields = c.String(),
                        EnglishName = c.String(),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            
            AddColumn("Customs.ClaimsRelatedEntities", "ContinuousRequestTypeCode", c => c.String(maxLength: 128));
            AddColumn("Customs.ClaimsRelatedEntities", "Explanation", c => c.String(maxLength: 256));
            AddColumn("Customs.ClaimsRelatedEntities", "Note", c => c.String(maxLength: 256));
            AddColumn("Customs.PhysicalChecks", "VehicleChassisNumber", c => c.String(maxLength: 20, unicode: false));
            CreateIndex("Customs.ClaimsRelatedEntities", "ContinuousRequestTypeCode");
            AddForeignKey("Customs.ClaimsRelatedEntities", "ContinuousRequestTypeCode", "dbo.ContinuousRequestTypes", "Code");
        }
        
        public override void Down()
        {
            AddColumn("Customs.Declarations", "MamanErrorXml", c => c.String(unicode: false));
            AddColumn("Customs.Declarations", "MamanStatusCode", c => c.String(maxLength: 3, unicode: false));
            DropForeignKey("Customs.VendorCommissions", "ModificationsTypeCode", "Customs.ModificationAndDiscountTypes");
            DropForeignKey("Customs.ProceduralFaults", "SignedByUserId", "dbo.Users");
            DropForeignKey("Customs.DeclarationMamanSpecialActions", "MamanSpecialActionStatusCode", "Customs.MamanSpecialActionStatuses");
            DropForeignKey("Customs.DeclarationMamanSpecialActions", "MamanSpecialActionCode", "Customs.MamanSpecialActions");
            DropForeignKey("Customs.DeclarationMamanSpecialActions", "DeclarationId", "Customs.Declarations");
            DropForeignKey("dbo.CustomsPartnerFtps", "FtpDetailsId", "dbo.FTPDetails");
            DropForeignKey("Customs.CourierMasters", "StorageSiteCode", "Customs.DeliverySiteTypes");
            DropForeignKey("Customs.CourierMasters", "IntegratorCode", "dbo.Cards");
            DropForeignKey("Customs.ClaimsRelatedEntities", "ContinuousRequestTypeCode", "dbo.ContinuousRequestTypes");
            DropIndex("Customs.VendorCommissions", new[] { "ModificationsTypeCode" });
            DropIndex("Customs.ProceduralFaults", new[] { "SignedByUserId" });
            DropIndex("Customs.DeclarationMamanSpecialActions", new[] { "MamanSpecialActionStatusCode" });
            DropIndex("Customs.DeclarationMamanSpecialActions", new[] { "MamanSpecialActionCode" });
            DropIndex("Customs.DeclarationMamanSpecialActions", new[] { "DeclarationId" });
            DropIndex("dbo.CustomsPartnerFtps", new[] { "FtpDetailsId" });
            DropIndex("Customs.CourierMasters", new[] { "IntegratorCode" });
            DropIndex("Customs.CourierMasters", new[] { "StorageSiteCode" });
            DropIndex("Customs.ClaimsRelatedEntities", new[] { "ContinuousRequestTypeCode" });
            DropPrimaryKey("Customs.VendorCommissions");
            DropColumn("Customs.VendorCommissions", "ModificationsTypeCode");
            DropColumn("dbo.TariffVersions", "InitialEnddate");
            DropColumn("Customs.SupplierInvoices", "ChangeInSupplierInvoice");
            DropColumn("Customs.ProceduralFaults", "SignedByUserId");
            DropColumn("Customs.PhysicalChecks", "VehicleChassisNumber");
            DropColumn("Customs.CustomsSettings", "CompanyType");
            DropColumn("Customs.InterfaceManagements", "InterfaceType");
            DropColumn("Customs.CourierMasters", "IntegratorCode");
            DropColumn("Customs.CourierMasters", "TruckerId");
            DropColumn("Customs.CourierMasters", "StorageSiteCode");
            DropColumn("Customs.ClaimsRelatedEntities", "Note");
            DropColumn("Customs.ClaimsRelatedEntities", "Explanation");
            DropColumn("Customs.ClaimsRelatedEntities", "ContinuousRequestTypeCode");
            DropColumn("Customs.Clients", "NationalIdentificationNumber");
            DropTable("dbo.TPGFileTypes");
            DropTable("Customs.PendingErrorPlaces");
            DropTable("Customs.DeclarationMamanSpecialActions");
            DropTable("Customs.MamanSpecialActionStatuses");
            DropTable("Customs.MamanSpecialActions");
            DropTable("dbo.CustomsPartnerFtps");
            DropTable("dbo.ContinuousRequestTypes");
            AddPrimaryKey("Customs.VendorCommissions", new[] { "VendorId", "CustomerId" });
            CreateIndex("Customs.Declarations", "MamanStatusCode");
            AddForeignKey("Customs.Declarations", "MamanStatusCode", "Customs.MamanStatuses", "Code");
        }
    }
}
