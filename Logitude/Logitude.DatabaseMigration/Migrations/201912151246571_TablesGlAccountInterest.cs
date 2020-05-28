namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TablesGlAccountInterest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GLAccountInterestPeriods",
                c => new
                {

                    GLAccountId = c.String(nullable: false, maxLength: 15, unicode: false),
                    LineNumber = c.Int(nullable: false),
                    Tenant = c.Int(nullable: false),
                    PeriodStartDate = c.DateTime(nullable: false),
                    StandardInterestRateBaseId = c.String(nullable: false, maxLength: 15, unicode: false),
                    StandardAddInterestPercent = c.Decimal(nullable: false, precision: 4, scale: 2),
                    ExceptionalInterestRateBaseId = c.String(nullable: false, maxLength: 15, unicode: false),
                    ExceptionalAddInterestPercent = c.Decimal(nullable: false, precision: 4, scale: 2),
                    CreditInterestRateBaseId = c.String(nullable: false, maxLength: 15, unicode: false),
                    CreditAddInterestPercent = c.Decimal(nullable: false, precision: 4, scale: 2),
                    CreateDateTime = c.DateTime(nullable: false),
                    UpdateDateTime = c.DateTime(nullable: false),
                    CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                    UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                })
                .PrimaryKey(t => new { t.GLAccountId, t.LineNumber })
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.InterestBasesTypes", t => t.CreditInterestRateBaseId)
                .ForeignKey("dbo.InterestBasesTypes", t => t.ExceptionalInterestRateBaseId)
                .ForeignKey("dbo.InterestBasesTypes", t => t.StandardInterestRateBaseId)
                .ForeignKey("dbo.GLAccounts", t => t.GLAccountId)

                .Index(t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.CreditInterestRateBaseId)
                .Index(t => t.ExceptionalInterestRateBaseId)
                .Index(t => t.StandardInterestRateBaseId)
                .Index(t => t.GLAccountId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.GLAccountInterestPeriods", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.GLAccountInterestPeriods", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.GLAccountInterestPeriods", "CreditInterestRateBaseId", "dbo.InterestBasesTypes");
            DropForeignKey("dbo.GLAccountInterestPeriods", "ExceptionalInterestRateBaseId", "dbo.InterestBasesTypes");
            DropForeignKey("dbo.GLAccountInterestPeriods", "StandardInterestRateBaseId", "dbo.InterestBasesTypes");
            DropForeignKey("dbo.GLAccountInterestPeriods", "GLAccountId", "dbo.GLAccounts");
            DropIndex("dbo.Users", new[] { "UpdatedByUserId" });
            DropIndex("dbo.Users", new[] { "CreatedByUserId" });
            DropIndex("dbo.InterestBasesTypes", new[] { "CreditInterestRateBaseId" });
            DropIndex("dbo.InterestBasesTypes", new[] { "ExceptionalInterestRateBaseId" });
            DropIndex("dbo.InterestBasesTypes", new[] { "StandardInterestRateBaseId" });
            DropIndex("dbo.GLAccounts", new[] { "GLAccountId" });
            DropTable("dbo.GLAccountInterestPeriods");
        }
    }
}
