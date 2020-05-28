namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRegionalTaxColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountingSettings", "AllowRegionalTaxManagement", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountingSettings", "AllowRegionalTaxManagement");
        }
    }
}
