namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsINTTRAOnlyDemo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TenantManagements", "IsINTTRAOnlyDemo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TenantManagements", "IsINTTRAOnlyDemo");
        }
    }
}
