namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamGlobalDBMigrationFixes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TenantManagements", "CountryName", c => c.String(maxLength: 120, unicode: false));
            AlterColumn("dbo.Settings", "StorageAccountKey", c => c.String(nullable: false, maxLength: 1000, unicode: false));
        }
        
        public override void Down()
        {
        }
    }
}
