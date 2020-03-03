namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamGlobalDBMigrationFixes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Settings", "StorageAccountKey", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Settings", "StorageAccountKey", c => c.String());
        }
    }
}
