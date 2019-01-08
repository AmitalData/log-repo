namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JLIsExternalReconcile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JournalLines", "IsExternalReconcile", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JournalLines", "IsExternalReconcile");
        }
    }
}
