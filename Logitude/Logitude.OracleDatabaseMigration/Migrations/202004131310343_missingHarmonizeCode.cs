namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missingHarmonizeCode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HarmonizeCodes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 6, unicode: false),
                        Description = c.String(nullable: false, maxLength: 1000, unicode: false),
                        ChapterCode = c.String(nullable: false, maxLength: 2, unicode: false),
                        ChapterDescription = c.String(nullable: false, maxLength: 1000, unicode: false),
                        SubChapterCode = c.String(nullable: false, maxLength: 4, unicode: false),
                        SubChapterDescription = c.String(nullable: false, maxLength: 1000, unicode: false),
                        SearchFields = c.String(nullable: false, maxLength: 1000),
                    })
                .PrimaryKey(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.HarmonizeCodes");
        }
    }
}
