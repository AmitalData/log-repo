namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDWCategoriesTables2_Rabaia : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DWCategories",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 50, unicode: false),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        Index = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.DWObjectFieldCategories",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DWObjectFieldId = c.String(nullable: false, maxLength: 15, unicode: false),
                        DWCategoryCode = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DWCategories", t => t.DWCategoryCode)
                .ForeignKey("dbo.DWObjectFields", t => t.DWObjectFieldId)
                .Index(t => t.DWObjectFieldId)
                .Index(t => t.DWCategoryCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DWObjectFieldCategories", "DWObjectFieldId", "dbo.DWObjectFields");
            DropForeignKey("dbo.DWObjectFieldCategories", "DWCategoryCode", "dbo.DWCategories");
            DropIndex("dbo.DWObjectFieldCategories", new[] { "DWCategoryCode" });
            DropIndex("dbo.DWObjectFieldCategories", new[] { "DWObjectFieldId" });
            DropTable("dbo.DWObjectFieldCategories");
            DropTable("dbo.DWCategories");
        }
    }
}
