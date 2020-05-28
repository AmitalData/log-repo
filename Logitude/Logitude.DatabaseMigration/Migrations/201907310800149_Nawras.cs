namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nawras : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.JournalExternalReconciles",
            //    c => new
            //        {
            //            JournalId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Line = c.Int(nullable: false),
            //            Tenant = c.Int(nullable: false),
            //            LedgerTransactionId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            ReconcileExternalPageLineId = c.String(nullable: false, maxLength: 15, unicode: false),
            //        })
            //    .PrimaryKey(t => new { t.JournalId, t.Line })
            //    .ForeignKey("dbo.Journals", t => t.JournalId)
            //    .ForeignKey("dbo.LedgerTransactions", t => t.LedgerTransactionId)
            //    .ForeignKey("dbo.ReconcileExternalPageLines", t => t.ReconcileExternalPageLineId)
            //    .Index(t => t.JournalId)
            //    .Index(t => t.LedgerTransactionId)
            //    .Index(t => t.ReconcileExternalPageLineId);
            
            //AddColumn("dbo.LedgerTransactions", "InProgressExternalReconcile", c => c.Boolean(nullable: false));
            //AddColumn("dbo.ReconcileExternalPageLines", "InProgressExternalReconcile", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JournalExternalReconciles", "ReconcileExternalPageLineId", "dbo.ReconcileExternalPageLines");
            DropForeignKey("dbo.JournalExternalReconciles", "LedgerTransactionId", "dbo.LedgerTransactions");
            DropForeignKey("dbo.JournalExternalReconciles", "JournalId", "dbo.Journals");
            DropIndex("dbo.JournalExternalReconciles", new[] { "ReconcileExternalPageLineId" });
            DropIndex("dbo.JournalExternalReconciles", new[] { "LedgerTransactionId" });
            DropIndex("dbo.JournalExternalReconciles", new[] { "JournalId" });
            DropColumn("dbo.ReconcileExternalPageLines", "InProgressExternalReconcile");
            DropColumn("dbo.LedgerTransactions", "InProgressExternalReconcile");
            DropTable("dbo.JournalExternalReconciles");
        }
    }
}
