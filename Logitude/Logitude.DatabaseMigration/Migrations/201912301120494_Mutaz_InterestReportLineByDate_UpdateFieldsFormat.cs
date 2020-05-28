namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_InterestReportLineByDate_UpdateFieldsFormat : DbMigration
    {
        public override void Up()
        {
          
            AlterColumn("dbo.InterestReportLinesByDates", "StandardInterestAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.InterestReportLinesByDates", "ExceptionalInterestAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.InterestReportLinesByDates", "CreditInterestAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.InterestReportLinesByDates", "CalculatedStandInterestAmount", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.InterestReportLinesByDates", "CalculatedExcepInterestAmount", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.InterestReportLinesByDates", "CalculatedCreditInterestAmount", c => c.Decimal(nullable: false, precision: 20, scale: 4));
          
        }
        
        public override void Down()
        {
            
         

            AlterColumn("dbo.InterestReportLinesByDates", "CalculatedCreditInterestAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.InterestReportLinesByDates", "CalculatedExcepInterestAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.InterestReportLinesByDates", "CalculatedStandInterestAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.InterestReportLinesByDates", "CreditInterestAmount", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.InterestReportLinesByDates", "ExceptionalInterestAmount", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.InterestReportLinesByDates", "StandardInterestAmount", c => c.Decimal(nullable: false, precision: 19, scale: 3));
   
           
        }
    }
}
