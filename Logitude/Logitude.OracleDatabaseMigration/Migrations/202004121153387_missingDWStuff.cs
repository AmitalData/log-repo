namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missingDWStuff : DbMigration
    {
        public override void Up()
        {
           
           
            CreateTable(
                "dbo.DWCategories",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 50, unicode: false),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        CatIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.DWObjectFieldCategories",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DWObjectFieldCode = c.String(nullable: false, maxLength: 50, unicode: false),
                        DWCategoryCode = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DWCategories", t => t.DWCategoryCode)
                .Index(t => t.DWCategoryCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DWObjectFieldCategories", "DWCategoryCode", "dbo.DWCategories");
            DropIndex("dbo.DWObjectFieldCategories", new[] { "DWCategoryCode" });
            DropTable("dbo.DWObjectFieldCategories");
            DropTable("dbo.DWCategories");
        }
    }
}
