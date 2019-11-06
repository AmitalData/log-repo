namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddDWHBuildStatusTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DWHBuildStatus",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 40, unicode: false),
                        LastIncrementalDWUpdateDate = c.DateTime(),
                        DWNextRunTime = c.DateTime(),
                        IsFullBuildDWRunning = c.Boolean(nullable: false),
                        IsIncrementalDWRunning = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DWHBuildStatus");
        }
    }
}
