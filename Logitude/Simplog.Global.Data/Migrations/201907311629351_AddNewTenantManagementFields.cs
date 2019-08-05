namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewTenantManagementFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TenantManagements", "TotalPrice", c => c.Double());
            AddColumn("dbo.TenantManagementLicenses", "FreeUsers", c => c.Int());
            AddColumn("dbo.TenantManagementLicenses", "Price", c => c.Double());
            AddColumn("dbo.TenantManagementLicenses", "TotalPrice", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TenantManagementLicenses", "TotalPrice");
            DropColumn("dbo.TenantManagementLicenses", "Price");
            DropColumn("dbo.TenantManagementLicenses", "FreeUsers");
            DropColumn("dbo.TenantManagements", "TotalPrice");
        }
    }
}
