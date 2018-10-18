namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBudgetandCategoryFieldsToTMProject_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TMProjects", "BudgetId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.TMProjects", "CategoryId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.TMProjects", "BudgetId");
            CreateIndex("dbo.TMProjects", "CategoryId");
            AddForeignKey("dbo.TMProjects", "BudgetId", "dbo.TMBudgets", "Id");
            AddForeignKey("dbo.TMProjects", "CategoryId", "dbo.TMProjectCategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TMProjects", "CategoryId", "dbo.TMProjectCategories");
            DropForeignKey("dbo.TMProjects", "BudgetId", "dbo.TMBudgets");
            DropIndex("dbo.TMProjects", new[] { "CategoryId" });
            DropIndex("dbo.TMProjects", new[] { "BudgetId" });
            DropColumn("dbo.TMProjects", "CategoryId");
            DropColumn("dbo.TMProjects", "BudgetId");
        }
    }
}
