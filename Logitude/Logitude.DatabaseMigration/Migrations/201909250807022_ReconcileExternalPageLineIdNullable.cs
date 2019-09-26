namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReconcileExternalPageLineIdNullable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.JournalExternalReconciles", new[] { "ReconcileExternalPageLineId" });
            AlterColumn("dbo.JournalExternalReconciles", "ReconcileExternalPageLineId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.JournalExternalReconciles", "ReconcileExternalPageLineId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.JournalExternalReconciles", new[] { "ReconcileExternalPageLineId" });
            AlterColumn("dbo.JournalExternalReconciles", "ReconcileExternalPageLineId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            CreateIndex("dbo.JournalExternalReconciles", "ReconcileExternalPageLineId");
        }
    }
}
