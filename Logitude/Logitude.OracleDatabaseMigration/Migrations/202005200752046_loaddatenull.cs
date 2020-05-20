namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class loaddatenull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Customs.Declarations", "LoadingDateTime", c => c.DateTime(precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("Customs.Declarations", "LoadingDateTime", c => c.DateTime(nullable: false, precision: 7));
        }
    }
}
