namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Table_InterestReportLines : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InterestReportLines",
                c => new
                    {
                        InterestReportId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        InterestTransactionId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.InterestReportId)
                .ForeignKey("dbo.InterestTransactions", t => t.InterestTransactionId)
                .Index(t => t.InterestTransactionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InterestReportLines", "InterestTransactionId", "dbo.InterestTransactions");
            DropIndex("dbo.InterestReportLines", new[] { "InterestTransactionId" });
            DropTable("dbo.InterestReportLines");
        }
    }
}
