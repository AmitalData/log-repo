namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_Fix_Fill_CustomerId_In_InterestReport : DbMigration
    {
        public override void Up()
        {
            Sql("update InterestReports set CustomerId = (select top 1 Cards.Id from Cards where GLAccountId = InterestReports.GLAccountId and Cards.Id in (select Id from customers) )");
            DropForeignKey("dbo.InterestReports", "CustomerId", "dbo.Cards");
            DropIndex("dbo.InterestReports", new[] { "CustomerId" });
            AlterColumn("dbo.InterestReports", "CustomerId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            CreateIndex("dbo.InterestReports", "CustomerId");
            AddForeignKey("dbo.InterestReports", "CustomerId", "dbo.Cards", "Id");

        }

        public override void Down()
        {
        }
    }
}
