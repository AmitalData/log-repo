namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddViewOrderToChargesGroup : DbMigration
    {
        public override void Up()
        {

            AddColumn("dbo.ChargesGroups", "ViewOrder", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChargesGroups", "ViewOrder");

        }
    }
}
