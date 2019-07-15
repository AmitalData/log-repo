namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMainAdditionalPackageApplied : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TenantManagements", "MainAdditionalPackageApplied", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TenantManagements", "MainAdditionalPackageApplied");
        }
    }
}
