namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddApplyVATSettingsForAllPartners_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tenants", "ApplyVATForAllPartners", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tenants", "ApplyVATForAllPartners");
        }
    }
}
