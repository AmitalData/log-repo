namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_AddIsCancelToInterestTransaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InterestTransactions", "IsCancelled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InterestTransactions", "IsCancelled");
        }
    }
}
