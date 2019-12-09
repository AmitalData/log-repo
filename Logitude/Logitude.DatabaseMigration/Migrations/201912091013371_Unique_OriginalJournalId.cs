namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Unique_OriginalJournalId : DbMigration
    {
        public override void Up()
        {
            Sql("CREATE UNIQUE NONCLUSTERED INDEX [UC_Journal_ORG] ON[dbo].[Journals]([OriginalJournalId] ASC) WHERE([OriginalJournalId] IS NOT NULL)");
        }
        
        public override void Down()
        {
        }
    }
}
