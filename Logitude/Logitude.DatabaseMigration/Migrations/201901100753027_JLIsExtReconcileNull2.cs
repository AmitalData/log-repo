namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JLIsExtReconcileNull2 : DbMigration
    {
        public override void Up()
        {

            Sql(@"update[dbo].[JournalLines] set[IsExternalReconcile] = 0 where[IsExternalReconcile] is null");
            AlterColumn("dbo.JournalLines", "IsExternalReconcile", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.JournalLines", "IsExternalReconcile", c => c.Boolean());
        }
    }
}
