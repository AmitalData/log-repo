namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAllowRegionalTaxColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChargesTypes", "ApplyRegionalTax", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AddColumn("dbo.AccountingSettings", "AllowRegionalTaxManagement", c => c.Boolean(nullable: false));
        }
    }
}
