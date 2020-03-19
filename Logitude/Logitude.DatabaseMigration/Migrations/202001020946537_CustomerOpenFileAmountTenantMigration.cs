namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerOpenFileAmountTenantMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerOpenFilesAmounts", "Tenant", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerOpenFilesAmounts", "Tenant");
        }
    }
}
