namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCurrencyTypeTenant : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.CurrencyTypeTenants",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        UpdateDate = c.DateTime(nullable: false, precision: 7),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Code = c.String(maxLength: 3, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Customs.CurrencyTypes", t => t.Code)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.Code);
            
            //AlterColumn("Customs.CargoSealIdentifiers", "CargoIdentifierKey1", c => c.String(maxLength: 35, unicode: false));
            //AlterColumn("Customs.CargoSealIdentifiers", "CargoIdentifierKey2", c => c.String(maxLength: 35, unicode: false));
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.CurrencyTypeTenants", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("Customs.CurrencyTypeTenants", "Code", "Customs.CurrencyTypes");
            DropIndex("Customs.CurrencyTypeTenants", new[] { "Code" });
            DropIndex("Customs.CurrencyTypeTenants", new[] { "UpdatedByUserId" });
            //AlterColumn("Customs.CargoSealIdentifiers", "CargoIdentifierKey2", c => c.String(nullable: false, maxLength: 35, unicode: false));
            //AlterColumn("Customs.CargoSealIdentifiers", "CargoIdentifierKey1", c => c.String(nullable: false, maxLength: 35, unicode: false));
            DropTable("Customs.CurrencyTypeTenants");
        }
    }
}
