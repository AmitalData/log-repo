namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestDateToQuoteTable_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quotes", "RequestDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Quotes", "RequestDate");
        }
    }
}
