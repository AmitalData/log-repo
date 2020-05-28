namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class objecttable_hashstring : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ObjectTables", "HashString", c => c.String());
           
        }
        
        public override void Down()
        {
            
            DropColumn("dbo.ObjectTables", "HashString");
        }
    }
}
