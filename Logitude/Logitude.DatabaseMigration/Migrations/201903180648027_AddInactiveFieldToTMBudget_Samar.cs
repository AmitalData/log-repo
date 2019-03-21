namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInactiveFieldToTMBudget_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TMBudgets", "Inactive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TMBudgets", "Inactive");
        }
    }
}
