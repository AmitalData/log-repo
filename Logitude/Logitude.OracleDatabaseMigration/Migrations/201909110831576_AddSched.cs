namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSched : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SchedulerLogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false, precision: 7),
                        Log = c.String(unicode: false),
                        HistoryId = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TaskSchedulerHistory", t => t.HistoryId)
                .Index(t => t.HistoryId);
            
            CreateTable(
                "dbo.SchedulerProcedure",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 100, unicode: false),
                        Name = c.String(maxLength: 100, unicode: false),
                        SearchFields = c.String(maxLength: 1000, unicode: false),
                        Description = c.String(maxLength: 1000, unicode: false),
                    })
                .PrimaryKey(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SchedulerLogs", "HistoryId", "dbo.TaskSchedulerHistory");
            DropIndex("dbo.SchedulerLogs", new[] { "HistoryId" });
            DropTable("dbo.SchedulerProcedure");
            DropTable("dbo.SchedulerLogs");
        }
    }
}
