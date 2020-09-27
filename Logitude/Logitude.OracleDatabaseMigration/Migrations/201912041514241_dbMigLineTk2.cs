namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbMigLineTk2 : DbMigration
    {
        public override void Up()
        {
            Sql("DROP INDEX IX_DBMigrationLines_Id ");
            RenameColumn(table: "Customs.DBMigrationLines", name: "Id", newName: "DBMigrationId");
            //RenameIndex(table: "Customs.DBMigrationLines", name: "IX_Id", newName: "IX_DBMigrationId");
            Sql ("CREATE INDEX IX_DBMigrationLines_Id ON DBMigrationLines (DBMigrationId)");
        }
        
        public override void Down()
        {
            RenameIndex(table: "Customs.DBMigrationLines", name: "IX_DBMigrationId", newName: "IX_Id");
            RenameColumn(table: "Customs.DBMigrationLines", name: "DBMigrationId", newName: "Id");
        }
    }
}
