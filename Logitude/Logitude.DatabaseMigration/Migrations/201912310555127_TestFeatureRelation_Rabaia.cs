namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestFeatureRelation_Rabaia : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.MenusTables", "FeatureId");
            //AddForeignKey("dbo.MenusTables", "FeatureId", "dbo.Features", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MenusTables", "FeatureId", "dbo.Features");
            DropIndex("dbo.MenusTables", new[] { "FeatureId" });
        }
    }
}
