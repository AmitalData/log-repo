namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddLastRunDetailTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BIReports", "LastRunByUserId", "dbo.Users");
            DropIndex("dbo.BIReports", new[] { "LastRunByUserId" });
            CreateTable(
                "dbo.LastRunDetails",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Tenant = c.Int(nullable: false),
                        LastRunDate = c.DateTime(nullable: false),
                        LastRunByUserId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.LastRunByUserId)
                .Index(t => t.LastRunByUserId);
            
            AddColumn("dbo.BIReports", "LastRunId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.BIReports", "LastRunId");
            //AddForeignKey("dbo.BIReports", "LastRunId", "dbo.LastRunDetails", "Id");
            Sql(@"  declare @BIReportId as varchar(15)
                    declare @Tenant as int
                    declare @LastRunDate as DateTime
                    declare @LastRunByUserId as varchar(15)
                    declare @NewEntityId as varchar(15)

                           DECLARE BIReportCursor CURSOR READ_ONLY
                           FOR
                           SELECT Id, Tenant, LastRunDate, LastRunByUserId
                           From BIReports
                           OPEN BIReportCursor FETCH NEXT FROM BIReportCursor INTO @BIReportId, @Tenant, @LastRunDate, @LastRunByUserId
                           WHILE @@FETCH_STATUS = 0
                           BEGIN

                                EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'LastRunDetail'
                                INSERT INTO LastRunDetails VALUES (@NewEntityId, @Tenant, @LastRunDate, @LastRunByUserId);
			                    UPDATE BIReports SET LastRunId = @NewEntityId where Id = @BIReportId

                           FETCH NEXT FROM BIReportCursor INTO @BIReportId, @Tenant, @LastRunDate, @LastRunByUserId

                           End
                           CLOSE BIReportCursor
                           DEALLOCATE BIReportCursor");
            DropColumn("dbo.BIReports", "LastRunDate");
            DropColumn("dbo.BIReports", "LastRunByUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BIReports", "LastRunByUserId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AddColumn("dbo.BIReports", "LastRunDate", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.BIReports", "LastRunId", "dbo.LastRunDetails");
            DropForeignKey("dbo.LastRunDetails", "LastRunByUserId", "dbo.Users");
            DropIndex("dbo.LastRunDetails", new[] { "LastRunByUserId" });
            DropIndex("dbo.BIReports", new[] { "LastRunId" });
            DropColumn("dbo.BIReports", "LastRunId");
            DropTable("dbo.LastRunDetails");
            CreateIndex("dbo.BIReports", "LastRunByUserId");
            AddForeignKey("dbo.BIReports", "LastRunByUserId", "dbo.Users", "Id");
        }
    }
}
