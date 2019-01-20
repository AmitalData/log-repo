namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddCreateDateToTableCustomerDepositions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerDepositions", "CreateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerDepositions", "CreateDate");
        }
    }
}
