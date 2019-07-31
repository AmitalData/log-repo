namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDefaultWarningPercentageToTenant_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tenants", "DefaultWarningPercentage", c => c.Double());          
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tenants", "DefaultWarningPercentage");
        }
    }
}
