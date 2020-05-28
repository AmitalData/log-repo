namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReconcileExternalPageLineIdNullabletk2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.JournalExternalReconciles", new[] { "LedgerTransactionId" });
            AlterColumn("dbo.JournalExternalReconciles", "LedgerTransactionId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.JournalExternalReconciles", "LedgerTransactionId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.JournalExternalReconciles", new[] { "LedgerTransactionId" });
            AlterColumn("dbo.JournalExternalReconciles", "LedgerTransactionId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            CreateIndex("dbo.JournalExternalReconciles", "LedgerTransactionId");
        }
    }
}
