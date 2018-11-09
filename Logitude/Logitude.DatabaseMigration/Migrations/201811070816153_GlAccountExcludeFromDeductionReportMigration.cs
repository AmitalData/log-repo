namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GlAccountExcludeFromDeductionReportMigration : DbMigration
    {
        public override void Up()
        {
            Sql("alter table glaccounts add  temp2 bit");
            Sql("update glaccounts set temp2 = IsPartOfDeductionReport");

            AddColumn("dbo.GLAccounts", "ExcludeFromDeductionReport", c => c.Boolean(nullable: false));
            DropColumn("dbo.GLAccounts", "IsPartOfDeductionReport");
            Sql("update GLAccounts set excludeFromdeductionreport = temp2");
            Sql("alter table glaccounts drop column temp2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GLAccounts", "IsPartOfDeductionReport", c => c.Boolean(nullable: false));
            DropColumn("dbo.GLAccounts", "ExcludeFromDeductionReport");
        }
    }
}
