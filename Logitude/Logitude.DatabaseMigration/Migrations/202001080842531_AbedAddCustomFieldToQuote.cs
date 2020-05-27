namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddCustomFieldToQuote : DbMigration
    {
        public override void Up()
        { 
            AddColumn("dbo.Quotes", "Field11", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field12", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field13", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field14", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field15", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field16", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field17", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field18", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field19", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field20", c => c.String(maxLength: 250));
       
        }
        
        public override void Down()
        {
     
            DropColumn("dbo.Quotes", "Field20");
            DropColumn("dbo.Quotes", "Field19");
            DropColumn("dbo.Quotes", "Field18");
            DropColumn("dbo.Quotes", "Field17");
            DropColumn("dbo.Quotes", "Field16");
            DropColumn("dbo.Quotes", "Field15");
            DropColumn("dbo.Quotes", "Field14");
            DropColumn("dbo.Quotes", "Field13");
            DropColumn("dbo.Quotes", "Field12");
            DropColumn("dbo.Quotes", "Field11");
         
        }
    }
}
