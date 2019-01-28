namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DisplayMemberPath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ObjectTables", "LovDisplayMemberPath", c => c.String());
            AddColumn("dbo.ObjectTables", "LovDisplayMemberPathLocal", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ObjectTables", "LovDisplayMemberPathLocal");
            DropColumn("dbo.ObjectTables", "LovDisplayMemberPath");
        }
    }
}
