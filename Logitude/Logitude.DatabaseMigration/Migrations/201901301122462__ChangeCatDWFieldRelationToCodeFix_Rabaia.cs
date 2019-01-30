namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _ChangeCatDWFieldRelationToCodeFix_Rabaia : DbMigration
    {
        public override void Up()
        {
            //Sql("alter table dbo.DWObjectFieldCategories drop constraint FK_dbo.DWObjectFieldCategories_dbo.DWObjectFields_DWObjectFieldId");
            DropForeignKey("dbo.DWObjectFieldCategories", "DWObjectFieldId", "dbo.DWObjectFields");
            DropForeignKey("dbo.DWObjectFieldCategories", "DWObjectFieldCode", "dbo.DWObjectFields");
            DropIndex("dbo.DWObjectFieldCategories", new[] { "DWObjectFieldCode" });
            AlterColumn("dbo.DWObjectFieldCategories", "DWObjectFieldCode", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DWObjectFieldCategories", "DWObjectFieldCode", c => c.String(nullable: false, maxLength: 15, unicode: false));
            CreateIndex("dbo.DWObjectFieldCategories", "DWObjectFieldCode");
            AddForeignKey("dbo.DWObjectFieldCategories", "DWObjectFieldCode", "dbo.DWObjectFields", "Id");
        }
    }
}
