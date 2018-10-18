namespace Logitude.SystemLogs.OracleMigratrion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class new_migrations : DbMigration
    {
        public override void Up()
        {
            return;
            CreateTable(
                "dbo.BatchServicesLogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 40, unicode: false),
                        BatchServiceCode = c.String(nullable: false, maxLength: 40, unicode: false),
                        LastActivity = c.DateTime(precision: 7),
                        CPU = c.Decimal(precision: 18, scale: 2),
                        CreateDate = c.DateTime(precision: 7),
                        NumberOfDoneItems = c.Int(),
                        DoneItemsInOneMinute = c.Int(nullable: false),
                        DoneItemsInFiveMinutes = c.Int(nullable: false),
                        DoneItemsInOneHour = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BatchServicesLogs");
        }
    }
}
