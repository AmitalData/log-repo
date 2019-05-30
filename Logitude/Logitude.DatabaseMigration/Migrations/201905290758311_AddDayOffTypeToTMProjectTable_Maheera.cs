namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDayOffTypeToTMProjectTable_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TMProjects", "DayOffTypeCode", c => c.String(maxLength: 3, unicode: false));
            CreateIndex("dbo.TMProjects", "DayOffTypeCode");
            AddForeignKey("dbo.TMProjects", "DayOffTypeCode", "dbo.TMDayOffTypes", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TMProjects", "DayOffTypeCode", "dbo.TMDayOffTypes");
            DropIndex("dbo.TMProjects", new[] { "DayOffTypeCode" });
            DropColumn("dbo.TMProjects", "DayOffTypeCode");
        }
    }
}
