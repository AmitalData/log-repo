namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Table_InterestReportLinesByDate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InterestReportLinesByDates",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        InterestReportId = c.String(nullable: false, maxLength: 15, unicode: false),
                        LineNumber = c.Decimal(nullable: false, precision: 6, scale: 0),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                        TotalInterestDays = c.Int(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AccumulatedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StandardInterestPercentage = c.Decimal(nullable: false, precision: 4, scale: 2),
                        ExceptionalInterestPercentage = c.Decimal(nullable: false, precision: 4, scale: 2),
                        CreditInterestPercentage = c.Decimal(nullable: false, precision: 4, scale: 2),
                        StandardInterestAmount = c.Decimal(nullable: false, precision: 19, scale: 3),
                        ExceptionalInterestAmount = c.Decimal(nullable: false, precision: 20, scale: 4),
                        CreditInterestAmount = c.Decimal(nullable: false, precision: 20, scale: 4),
                        CalculatedStandInterestAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CalculatedExcepInterestAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CalculatedCreditInterestAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CalculationDetails = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.InterestReportLinesByDates");
        }
    }
}
