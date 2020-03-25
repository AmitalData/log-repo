namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Abed_AddLastRunDetailToContext : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BIReports", "LastRunId", "dbo.LastRunDetails");
            DropIndex("dbo.BIReports", new[] { "LastRunId" });
            DropIndex("dbo.LastRunDetails", new[] { "LastRunByUserId" });
            DropPrimaryKey("dbo.LastRunDetails");

            Sql("EXEC('declare @sql nvarchar(2000) declare @defConName varchar(500) set @defConName = (select con.name from sys.default_constraints con left outer join sys.objects t on con.parent_object_id = t.object_id left outer join sys.all_columns col on con.parent_column_id = col.column_id and con.parent_object_id = col.object_id where schema_name(t.schema_id) = ''dbo'' and t.name = ''BIReports'' and col.name = ''LastRunId'') set @sql = ''alter table [dbo].[BIReports] drop constraint ['' + @defConName + '']'' exec(@sql)');");

            AlterColumn("dbo.BIReports", "LastRunId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.LastRunDetails", "Id", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.LastRunDetails", "LastRunByUserId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AddPrimaryKey("dbo.LastRunDetails", "Id");
            CreateIndex("dbo.BIReports", "LastRunId");
            CreateIndex("dbo.LastRunDetails", "LastRunByUserId");
            AddForeignKey("dbo.BIReports", "LastRunId", "dbo.LastRunDetails", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BIReports", "LastRunId", "dbo.LastRunDetails");
            DropIndex("dbo.LastRunDetails", new[] { "LastRunByUserId" });
            DropIndex("dbo.BIReports", new[] { "LastRunId" });
            DropPrimaryKey("dbo.LastRunDetails");
            AlterColumn("dbo.LastRunDetails", "LastRunByUserId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.LastRunDetails", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.BIReports", "LastRunId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.LastRunDetails", "Id");
            CreateIndex("dbo.LastRunDetails", "LastRunByUserId");
            CreateIndex("dbo.BIReports", "LastRunId");
            AddForeignKey("dbo.BIReports", "LastRunId", "dbo.LastRunDetails", "Id");
        }
    }
}
