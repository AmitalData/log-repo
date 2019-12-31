namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropRelationMenuTablesWithFeatureNew_Rabaia : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MenusTables", "FeatureId", "dbo.Features");
            DropIndex("dbo.MenusTables", new[] { "FeatureId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.MenusTables", "FeatureId");
            AddForeignKey("dbo.MenusTables", "FeatureId", "dbo.Features", "Id");
        }
    }
}
