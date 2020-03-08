namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigrationCargoSealIdentifiers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.AmendmentTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 2, unicode: false),
                        EnglishName = c.String(maxLength: 100, unicode: false),
                        SearchFields = c.String(maxLength: 1000),
                        LocalName = c.String(maxLength: 100),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "Customs.CargoSealIdentifiers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        DeclarationId = c.String(maxLength: 15, unicode: false),
                        CargoRowNumber = c.String(maxLength: 9, unicode: false),
                        ContainerNumber = c.String(maxLength: 11, unicode: false),
                        UpdateDate = c.DateTime(precision: 7),
                        ImporterId = c.String(maxLength: 15, unicode: false),
                        CargoIdentifierTypeCode = c.String(maxLength: 4, unicode: false),
                        CargoIdentifierKey1 = c.String(maxLength: 35, unicode: false),
                        CargoIdentifierKey2 = c.String(maxLength: 35, unicode: false),
                        CargoIdentifierKey3 = c.String(maxLength: 35, unicode: false),
                        Status = c.String(maxLength: 1, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Customs.CargoIdentifireTypes", t => t.CargoIdentifierTypeCode)
                .ForeignKey("Customs.Clients", t => t.ImporterId)
                .ForeignKey("Customs.Declarations", t => t.DeclarationId)
                .Index(t => t.DeclarationId)
                .Index(t => t.ImporterId)
                .Index(t => t.CargoIdentifierTypeCode);
            
            CreateTable(
                "Customs.SealTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 3, unicode: false),
                        LocalName = c.String(maxLength: 40),
                        EnglishName = c.String(maxLength: 40),
                        SearchFields = c.String(maxLength: 1000),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "Customs.SealUpdateReasonTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 3, unicode: false),
                        EnglishName = c.String(maxLength: 40, unicode: false),
                        SearchFields = c.String(maxLength: 1000),
                        LocalName = c.String(maxLength: 40),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.CargoSealIdentifiers", "DeclarationId", "Customs.Declarations");
            DropForeignKey("Customs.CargoSealIdentifiers", "ImporterId", "Customs.Clients");
            DropForeignKey("Customs.CargoSealIdentifiers", "CargoIdentifierTypeCode", "Customs.CargoIdentifireTypes");
            DropIndex("Customs.CargoSealIdentifiers", new[] { "CargoIdentifierTypeCode" });
            DropIndex("Customs.CargoSealIdentifiers", new[] { "ImporterId" });
            DropIndex("Customs.CargoSealIdentifiers", new[] { "DeclarationId" });
            DropTable("Customs.SealUpdateReasonTypes");
            DropTable("Customs.SealTypes");
            DropTable("Customs.CargoSealIdentifiers");
            DropTable("Customs.AmendmentTypes");
        }
    }
}
