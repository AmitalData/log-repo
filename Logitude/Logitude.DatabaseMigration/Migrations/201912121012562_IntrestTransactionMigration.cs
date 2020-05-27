namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntrestTransactionMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InterestTransactions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        UpdateDateTime = c.DateTime(nullable: false),
                        SearchFields = c.String(),
                        GLAccountId = c.String(maxLength: 15, unicode: false),
                        InterestEntityTypeCode = c.String(nullable: false, maxLength: 15, unicode: false),
                        EntityId = c.String(nullable: false, maxLength: 15, unicode: false),
                        OriginalEntityLineNumber = c.Int(nullable: false),
                        LocalAmount = c.Decimal(nullable: false, precision: 16, scale: 2),
                        ForeignAmount = c.Decimal(precision: 16, scale: 2),
                        CurrencyId = c.String(maxLength: 15, unicode: false),
                        InterestValueDate = c.DateTime(nullable: false),
                        InterestReportId = c.String(maxLength: 15, unicode: false),
                        IsClosed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currencies", t => t.CurrencyId)
                .ForeignKey("dbo.GLAccounts", t => t.GLAccountId)
                .Index(t => t.GLAccountId)
                .Index(t => t.CurrencyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InterestTransactions", "GLAccountId", "dbo.GLAccounts");
            DropForeignKey("dbo.InterestTransactions", "CurrencyId", "dbo.Currencies");
            DropIndex("dbo.InterestTransactions", new[] { "CurrencyId" });
            DropIndex("dbo.InterestTransactions", new[] { "GLAccountId" });
            DropTable("dbo.InterestTransactions");
        }
    }
}
