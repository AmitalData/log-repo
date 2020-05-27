namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Journals_IsLedgerCreated : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Journals", "IsLedgerCreated", false, "IX_Journals_IsLedgerCreated");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Journals", "IX_Journals_IsLedgerCreated");
        }
    }
}
