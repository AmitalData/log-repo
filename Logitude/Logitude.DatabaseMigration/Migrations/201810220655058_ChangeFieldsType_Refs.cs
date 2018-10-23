namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeFieldsType_Refs : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LedgerTransactions", "Reference1", c => c.String(maxLength: 30));
            AlterColumn("dbo.LedgerTransactions", "Reference2", c => c.String(maxLength: 30));
            AlterColumn("dbo.LedgerTransactions", "Reference3", c => c.String(maxLength: 30));
            AlterColumn("dbo.JournalLines", "Reference1", c => c.String(maxLength: 30));
            AlterColumn("dbo.JournalLines", "Reference2", c => c.String(maxLength: 30));
            AlterColumn("dbo.JournalLines", "Reference3", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.JournalLines", "Reference3", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.JournalLines", "Reference2", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.JournalLines", "Reference1", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.LedgerTransactions", "Reference3", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.LedgerTransactions", "Reference2", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.LedgerTransactions", "Reference1", c => c.String(maxLength: 30, unicode: false));
        }
    }
}
