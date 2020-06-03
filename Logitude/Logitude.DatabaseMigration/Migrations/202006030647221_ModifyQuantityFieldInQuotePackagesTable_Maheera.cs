namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyQuantityFieldInQuotePackagesTable_Maheera : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.QuotePackages", "Quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.QuotePackages", "Quantity", c => c.Int());
        }
    }
}
