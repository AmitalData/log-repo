namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JornalMoreDATA : DbMigration
    {
        public override void Up()
        {
            //DropIndex("dbo.Activities", new[] { "OwnerId" });
            //DropIndex("dbo.Activities", new[] { "BusinessUnitId" });
            //DropIndex("Customs.Vehicles", new[] { "ImporterIdentityId" });
            //CreateTable(
            //    "Customs.CustomsAirlines",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            AirlineCode = c.String(nullable: false, maxLength: 2, unicode: false),
            //            LocalName = c.String(maxLength: 100),
            //            EnglishName = c.String(maxLength: 70, unicode: false),
            //            InActive = c.Boolean(nullable: false),
            //            SearchFields = c.String(maxLength: 500),
            //            AirlinePrefix = c.String(nullable: false, maxLength: 3, unicode: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //AddColumn("Customs.Vehicles", "TaxiMedalOwner", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("Customs.Vehicles", "ImporterPassportNumber", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("Customs.Vehicles", "ImporterPassCountryCode", c => c.String(maxLength: 2, unicode: false));
            //AddColumn("Customs.Vehicles", "ImporterPassportTypeCode", c => c.String(maxLength: 2, unicode: false));
            //AddColumn("Customs.Vehicles", "IsCBS", c => c.Boolean(nullable: false));
            //AddColumn("Customs.Vehicles", "IsSlipperClutch", c => c.Boolean(nullable: false));
            //AddColumn("Customs.Vehicles", "IsSteeringDamper", c => c.Boolean(nullable: false));
            //AddColumn("Customs.Vehicles", "IsTCS", c => c.Boolean(nullable: false));
            //AddColumn("Customs.Vehicles", "IsTPS", c => c.Boolean(nullable: false));
            //AddColumn("Customs.Vehicles", "VehicleCategory", c => c.String(maxLength: 3, unicode: false));
            //AddColumn("Customs.Vehicles", "VehicleMaxPowerKW", c => c.Decimal(precision: 7, scale: 2));
            //AddColumn("Customs.VehicleOwners", "PassportNumber", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("Customs.VehicleOwners", "PassCountryCode", c => c.String(maxLength: 2, unicode: false));
            //AddColumn("Customs.VehicleOwners", "ImporterPassportTypeCode", c => c.String(maxLength: 2, unicode: false));
            //AlterColumn("dbo.Activities", "OwnerId", c => c.String(maxLength: 15, unicode: false));
            //AlterColumn("dbo.Activities", "BusinessUnitId", c => c.String(maxLength: 50, unicode: false));
            //AlterColumn("Customs.Claims", "SearchFields", c => c.String(maxLength: 2000));
            //AlterColumn("Customs.Vehicles", "VehiclePowerKW", c => c.Decimal(precision: 12, scale: 2));
            //AlterColumn("Customs.Vehicles", "ImporterIdentityId", c => c.String(maxLength: 15, unicode: false));
            //CreateIndex("dbo.Activities", "OwnerId");
            //CreateIndex("dbo.Activities", "BusinessUnitId");
            //CreateIndex("Customs.Vehicles", "ImporterIdentityId");
            //CreateIndex("Customs.Vehicles", "ImporterPassCountryCode");
            //CreateIndex("Customs.Vehicles", "ImporterPassportTypeCode");
            //CreateIndex("Customs.VehicleOwners", "PassCountryCode");
            //CreateIndex("Customs.VehicleOwners", "ImporterPassportTypeCode");
            //AddForeignKey("Customs.Vehicles", "ImporterPassCountryCode", "Customs.CustomsCountries", "Code");
            //AddForeignKey("Customs.Vehicles", "ImporterPassportTypeCode", "Customs.PassportTypes", "Code");
            //AddForeignKey("Customs.VehicleOwners", "PassCountryCode", "Customs.CustomsCountries", "Code");
            //AddForeignKey("Customs.VehicleOwners", "ImporterPassportTypeCode", "Customs.PassportTypes", "Code");
        }
        
        public override void Down()
        {
            //DropForeignKey("Customs.VehicleOwners", "ImporterPassportTypeCode", "Customs.PassportTypes");
            //DropForeignKey("Customs.VehicleOwners", "PassCountryCode", "Customs.CustomsCountries");
            //DropForeignKey("Customs.Vehicles", "ImporterPassportTypeCode", "Customs.PassportTypes");
            //DropForeignKey("Customs.Vehicles", "ImporterPassCountryCode", "Customs.CustomsCountries");
            //DropIndex("Customs.VehicleOwners", new[] { "ImporterPassportTypeCode" });
            //DropIndex("Customs.VehicleOwners", new[] { "PassCountryCode" });
            //DropIndex("Customs.Vehicles", new[] { "ImporterPassportTypeCode" });
            //DropIndex("Customs.Vehicles", new[] { "ImporterPassCountryCode" });
            //DropIndex("Customs.Vehicles", new[] { "ImporterIdentityId" });
            //DropIndex("dbo.Activities", new[] { "BusinessUnitId" });
            //DropIndex("dbo.Activities", new[] { "OwnerId" });
            //AlterColumn("Customs.Vehicles", "ImporterIdentityId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            //AlterColumn("Customs.Vehicles", "VehiclePowerKW", c => c.Int());
            //AlterColumn("Customs.Claims", "SearchFields", c => c.String(maxLength: 1000));
            //AlterColumn("dbo.Activities", "BusinessUnitId", c => c.String(nullable: false, maxLength: 50, unicode: false));
            //AlterColumn("dbo.Activities", "OwnerId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            //DropColumn("Customs.VehicleOwners", "ImporterPassportTypeCode");
            //DropColumn("Customs.VehicleOwners", "PassCountryCode");
            //DropColumn("Customs.VehicleOwners", "PassportNumber");
            //DropColumn("Customs.Vehicles", "VehicleMaxPowerKW");
            //DropColumn("Customs.Vehicles", "VehicleCategory");
            //DropColumn("Customs.Vehicles", "IsTPS");
            //DropColumn("Customs.Vehicles", "IsTCS");
            //DropColumn("Customs.Vehicles", "IsSteeringDamper");
            //DropColumn("Customs.Vehicles", "IsSlipperClutch");
            //DropColumn("Customs.Vehicles", "IsCBS");
            //DropColumn("Customs.Vehicles", "ImporterPassportTypeCode");
            //DropColumn("Customs.Vehicles", "ImporterPassCountryCode");
            //DropColumn("Customs.Vehicles", "ImporterPassportNumber");
            //DropColumn("Customs.Vehicles", "TaxiMedalOwner");
            //DropTable("Customs.CustomsAirlines");
            //CreateIndex("Customs.Vehicles", "ImporterIdentityId");
            //CreateIndex("dbo.Activities", "BusinessUnitId");
            //CreateIndex("dbo.Activities", "OwnerId");
        }
    }
}
