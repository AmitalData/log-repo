namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WithholdingTaxDeductionTypeMigration : DbMigration
    {
        public override void Up()
        {
        
            AlterColumn("dbo.WithholdingTaxDeductionTypes", "LocalName", c => c.String(nullable: false, maxLength: 120));
            Sql("update GLAccounts set DeductionFileTypeId= null ");
            Sql("delete from WithholdingTaxDeductionTypes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AirlineAreasPorts", "Description", c => c.String());
            AddColumn("dbo.Tenants", "DefaultWarningPercentage", c => c.Double());
            AlterColumn("dbo.OccasionTypes", "Name", c => c.String(maxLength: 100));
            AlterColumn("dbo.OccasionTypes", "Code", c => c.String(maxLength: 3, unicode: false));
            AlterColumn("dbo.WithholdingTaxDeductionTypes", "LocalName", c => c.String(nullable: false, maxLength: 60));
            DropColumn("dbo.TariffSettings", "DefaultWarningPercentage");
        }
    }
}
