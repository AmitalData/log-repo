namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BIReportLastRunFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BIReports", "LastRunDate", c => c.DateTime(nullable: true));
            Sql(@"update BIReports set LastRunDate = UpdateDate ");
            AlterColumn("dbo.BIReports", "LastRunDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.BIReports", "LastRunByUserId", c => c.String(nullable: true, maxLength: 15, unicode: false));
            Sql(@"update BIReports set LastRunByUserId = UpdatedByUserId");
            AlterColumn("dbo.BIReports", "LastRunByUserId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            CreateIndex("dbo.BIReports", "LastRunByUserId");
            AddForeignKey("dbo.BIReports", "LastRunByUserId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BIReports", "LastRunByUserId", "dbo.Users");
            DropIndex("dbo.BIReports", new[] { "LastRunByUserId" });
            DropColumn("dbo.BIReports", "LastRunByUserId");
            DropColumn("dbo.BIReports", "LastRunDate");
        }
    }
}
