namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class New_Module_Tariff_DB_4_Tables_Khalid : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TariffLines",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        SearchFields = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                        TariffId = c.String(maxLength: 15, unicode: false),
                        Version = c.Int(nullable: false),
                        MinPrice = c.Int(),
                        Step1Price = c.Int(),
                        Step2Price = c.Int(),
                        Step3Price = c.Int(),
                        Step4Price = c.Int(),
                        Step5Price = c.Int(),
                        Step6Price = c.Int(),
                        Step7Price = c.Int(),
                        Step8Price = c.Int(),
                        OriginPortId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tariffs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        SearchFields = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        InActive = c.Boolean(nullable: false),
                        Description = c.String(maxLength: 250),
                        SellerId = c.String(nullable: false, maxLength: 15, unicode: false),
                        CurrencyId = c.String(nullable: false, maxLength: 15, unicode: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        LastExpirationDate = c.DateTime(),
                        PriceSteps = c.String(maxLength: 100, unicode: false),
                        TypeCode = c.String(maxLength: 3, unicode: false),
                        LastStartDate = c.DateTime(),
                        LastVersion = c.Int(nullable: false),
                        ContractNumber = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
            CreateTable(
                "dbo.TariffTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 3, unicode: false),
                        Name = c.String(maxLength: 100, unicode: false),
                        SearchFields = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.TariffVersions",
                c => new
                    {
                        TariffId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Version = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        SearchFields = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.TariffId, t.Version })
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .Index(t => t.CreatedByUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TariffVersions", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Tariffs", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Tariffs", "CreatedByUserId", "dbo.Users");
            DropIndex("dbo.TariffVersions", new[] { "CreatedByUserId" });
            DropIndex("dbo.Tariffs", new[] { "UpdatedByUserId" });
            DropIndex("dbo.Tariffs", new[] { "CreatedByUserId" });
            DropColumn("dbo.ARPayments", "IsExternalEntity");
            DropColumn("dbo.States", "QBOTransactionLocationCode");
            DropTable("dbo.TariffVersions");
            DropTable("dbo.TariffTypes");
            DropTable("dbo.Tariffs");
            DropTable("dbo.TariffLines");
        }
    }
}
