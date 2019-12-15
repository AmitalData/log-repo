namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alter_Field_InterestCalculationStartDate_Nullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GLAccounts", "InterestCalculationStartDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GLAccounts", "InterestCalculationStartDate", c => c.DateTime(nullable: false));
        }
    }
}
