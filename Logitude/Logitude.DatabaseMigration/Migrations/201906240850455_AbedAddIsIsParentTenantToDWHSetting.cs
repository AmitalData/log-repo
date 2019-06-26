namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddIsIsParentTenantToDWHSetting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DWHSettings", "IsParentTenant", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DWHSettings", "IsParentTenant");
        }
    }
}
