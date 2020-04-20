namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyQuoteCustomerNameField_Maheera : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Quotes", "CustomerName", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Quotes", "CustomerName", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
