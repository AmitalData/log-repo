namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missingcusoms22 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.CurrencyTypeTenants",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Code = c.String(maxLength: 3, unicode: false),
                        TenantInactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Customs.CurrencyTypes", t => t.Code)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.CurrencyTypeTenants", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("Customs.CurrencyTypeTenants", "Code", "Customs.CurrencyTypes");
            DropIndex("Customs.CurrencyTypeTenants", new[] { "Code" });
            DropIndex("Customs.CurrencyTypeTenants", new[] { "UpdatedByUserId" });
            DropTable("Customs.CurrencyTypeTenants");
        }
    }
}
