namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddSearchFieldsToCustomsShipper : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomsShippers", "SearchFields", c => c.String(maxLength: 1000, unicode: false));
     
        }
        
        public override void Down()
        {
       
            DropColumn("dbo.CustomsShippers", "SearchFields");
        }
    }
}
