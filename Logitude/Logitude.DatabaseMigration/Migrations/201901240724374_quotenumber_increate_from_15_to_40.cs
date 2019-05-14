namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class quotenumber_increate_from_15_to_40 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Quotes", "QuoteNumber", c => c.String(nullable: false, maxLength: 40, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Quotes", "QuoteNumber", c => c.String(nullable: false, maxLength: 15, unicode: false));
        }
    }
}
