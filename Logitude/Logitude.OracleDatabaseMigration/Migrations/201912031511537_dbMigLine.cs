namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbMigLine : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.DBMigrationLines",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        CounterKey = c.Int(nullable: false),
                        SqlScript = c.String(nullable: false, maxLength: 1024, unicode: false),
                        ApprovedRemarks = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => new { t.Id, t.CounterKey })
                .ForeignKey("Customs.DBMigrations", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "Customs.DBMigrations",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        ExecuteDate = c.DateTime(nullable: false, precision: 7),
                        MajorVersion = c.Decimal(nullable: false, precision: 5, scale: 2),
                        MinorVersion = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 256),
                        IsClose = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.DBMigrationLines", "Id", "Customs.DBMigrations");
            DropIndex("Customs.DBMigrationLines", new[] { "Id" });
            DropTable("Customs.DBMigrations");
            DropTable("Customs.DBMigrationLines");
        }
    }
}
