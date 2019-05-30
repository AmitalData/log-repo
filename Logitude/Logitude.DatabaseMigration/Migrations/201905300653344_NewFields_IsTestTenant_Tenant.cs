namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewFields_IsTestTenant_Tenant : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tenants", "IsTestTenant", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tenants", "IsTestTenant");
        }
    }
}
