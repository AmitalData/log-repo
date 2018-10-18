namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTMProjectTable_Maheera1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TMProjects", new[] { "BudgetId" });
            DropIndex("dbo.TMProjects", new[] { "CategoryId" });
            AlterColumn("dbo.TMProjects", "BudgetId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.TMProjects", "CategoryId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            CreateIndex("dbo.TMProjects", "BudgetId");
            CreateIndex("dbo.TMProjects", "CategoryId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TMProjects", new[] { "CategoryId" });
            DropIndex("dbo.TMProjects", new[] { "BudgetId" });
            AlterColumn("dbo.TMProjects", "CategoryId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.TMProjects", "BudgetId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.TMProjects", "CategoryId");
            CreateIndex("dbo.TMProjects", "BudgetId");
        }
    }
}
