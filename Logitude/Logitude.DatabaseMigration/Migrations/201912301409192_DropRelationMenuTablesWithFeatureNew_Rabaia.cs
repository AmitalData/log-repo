namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropRelationMenuTablesWithFeatureNew_Rabaia : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE MenusTables DROP CONSTRAINT  FK_MenusTableFeature");
            Sql("ALTER TABLE Queries DROP CONSTRAINT  FK_QueryFeature; ");
            Sql("ALTER TABLE ObjectTableTabs DROP CONSTRAINT  FK_ObjectTableTabFeature; ");
            Sql("ALTER TABLE Reports DROP CONSTRAINT[FK_dbo.Reports_dbo.Features_FeatureId]");
            Sql("ALTER TABLE MenuButtons DROP CONSTRAINT  FK_MenuButtonFeature; ");
            Sql("ALTER TABLE [dbo].[PackageFeatures] DROP CONSTRAINT [FK_PackageFeatureFeature]");
            Sql("ALTER TABLE [dbo].[RoleFeatures] DROP CONSTRAINT [FK_RoleFeatureFeature]");
            
            //DropForeignKey("dbo.MenusTables", "FeatureId", "dbo.Features");
            //DropIndex("dbo.MenusTables", new[] { "FeatureId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.MenusTables", "FeatureId");
            AddForeignKey("dbo.MenusTables", "FeatureId", "dbo.Features", "Id");
        }
    }
}
