namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeTarrifConcurrencyGUIDRequired : DbMigration
    {
        public override void Up()
        {
            Sql("update Tariffs set ConcurrencyGUID = NEWID() where ConcurrencyGUID is null");

            AlterColumn("dbo.Tariffs", "ConcurrencyGUID", c => c.String(nullable: false, maxLength: 40, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tariffs", "ConcurrencyGUID", c => c.String(maxLength: 40, unicode: false));
        }
    }
}
