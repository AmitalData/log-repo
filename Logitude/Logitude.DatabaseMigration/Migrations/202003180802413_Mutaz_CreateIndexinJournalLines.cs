namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_CreateIndexinJournalLines : DbMigration
    {
        public override void Up()
        {
            Sql("CREATE NONCLUSTERED INDEX [IX_JournalLines_ExternalReconcileNumber] ON [dbo].[JournalLines]([ExternalReconcileNumber] ASC)");
        }

        public override void Down()
        {
        }
    }
}
