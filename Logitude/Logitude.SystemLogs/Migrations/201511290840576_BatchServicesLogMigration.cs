namespace Logitude.SystemLogs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BatchServicesLogMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BatchServicesLogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 40, unicode: false),
                        BatchServiceCode = c.String(maxLength: 40, unicode: false),
                        LastActivity = c.String(unicode: false),
                        CPU = c.Decimal(precision: 18, scale: 2),
                        CreateDate = c.DateTime(),
                        NumberOfDoneItems = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BatchServicesLogs");
        }
    }
}
