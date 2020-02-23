namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_Update_Field_CustomerId_In_InterestReport : DbMigration
    {
        public override void Up()
        {
            Sql("update InterestReports set CustomerId = (select Cards.Id from Cards where GLAccountId = InterestReports.GLAccountId)");
            AlterColumn("dbo.InterestReports", "CustomerId", c => c.String(maxLength: 15, unicode: false));
        }

        public override void Down()
        {
        }
    }
}
