namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Field_InterestCreditLimit_To_GLAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GLAccounts", "InterestCreditLimit", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GLAccounts", "InterestCreditLimit");
        }
    }
}
