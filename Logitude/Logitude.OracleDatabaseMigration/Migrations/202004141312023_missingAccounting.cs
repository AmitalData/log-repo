namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missingAccounting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IntegrityCheckStatuses",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 1, unicode: false),
                        Name = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.InterestEntityTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 1, unicode: false),
                        EnglishName = c.String(maxLength: 100, unicode: false),
                        SearchFields = c.String(),
                        LocalName = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.InterestReportStatuses",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 4, unicode: false),
                        LocalName = c.String(maxLength: 30),
                        SearchFields = c.String(),
                        EnglishName = c.String(nullable: false, maxLength: 60, unicode: false),
                    })
                .PrimaryKey(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.InterestReportStatuses");
            DropTable("dbo.InterestEntityTypes");
            DropTable("dbo.IntegrityCheckStatuses");
        }
    }
}
