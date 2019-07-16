namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fuuscreenabdullah : DbMigration
    {
        public override void Up()
        {
            //RenameColumn(table: "dbo.Reconciliations", name: "RNumber", newName: "Number");
            AddColumn("dbo.ObjectFields", "EnableFullscreenTextBox", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ObjectFields", "EnableFullscreenTextBox");
            //RenameColumn(table: "dbo.Reconciliations", name: "Number", newName: "RNumber");
        }
    }
}
