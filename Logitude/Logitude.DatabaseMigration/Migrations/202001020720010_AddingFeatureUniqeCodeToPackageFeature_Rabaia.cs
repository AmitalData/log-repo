namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingFeatureUniqeCodeToPackageFeature_Rabaia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PackageFeatures", "FeatureUniqeCode", c => c.String(nullable: true, maxLength: 120, unicode: false));
            Sql("update dbo.PackageFeatures set FeatureUniqeCode = (select FeatureUniqeCode from dbo.Features where Id = PackageFeatures.FeatureId)");
            AlterColumn("dbo.PackageFeatures", "FeatureUniqeCode", c => c.String(nullable: true, maxLength: 120, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PackageFeatures", "FeatureUniqeCode");
        }
    }
}
