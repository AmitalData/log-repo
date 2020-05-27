namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInProgressExternalReconcile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LedgerTransactions", "InProgressExternalReconcile", c => c.Boolean(nullable: false));
            AddColumn("dbo.ReconcileExternalPageLines", "InProgressExternalReconcile", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReconcileExternalPageLines", "InProgressExternalReconcile");
            DropColumn("dbo.LedgerTransactions", "InProgressExternalReconcile");
        }
    }
}
