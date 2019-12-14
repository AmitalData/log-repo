namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntrestEntityTypeMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InterestEntityTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 1, unicode: false),
                        EnglishName = c.String(maxLength: 100, unicode: false),
                        SearchFields = c.String(),
                        LocalName = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Code);
            
            AlterColumn("dbo.InterestTransactions", "InterestEntityTypeCode", c => c.String(nullable: false, maxLength: 1, unicode: false));
            CreateIndex("dbo.InterestTransactions", "InterestEntityTypeCode");
            AddForeignKey("dbo.InterestTransactions", "InterestEntityTypeCode", "dbo.InterestEntityTypes", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InterestTransactions", "InterestEntityTypeCode", "dbo.InterestEntityTypes");
            DropIndex("dbo.InterestTransactions", new[] { "InterestEntityTypeCode" });
            AlterColumn("dbo.InterestTransactions", "InterestEntityTypeCode", c => c.String(nullable: false, maxLength: 15, unicode: false));
            DropTable("dbo.InterestEntityTypes");
        }
    }
}
