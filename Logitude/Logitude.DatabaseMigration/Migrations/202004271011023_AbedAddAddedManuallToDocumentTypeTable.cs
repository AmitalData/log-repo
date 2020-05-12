namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddAddedManuallToDocumentTypeTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentTypes", "AddedManually", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DocumentTypes", "AddedManually");
        }
    }
}
