namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddMarkForDeleteFieldToDocumentsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "MarkForDelete", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "MarkForDelete");
        }
    }
}
