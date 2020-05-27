namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAllowCustomersInAgentsLOV_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tenants", "AllowCustomersInAgentsLOV", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tenants", "AllowCustomersInAgentsLOV");
        }
    }
}
