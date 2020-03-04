namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingFeatureUniqeCodeToRoleFeatures_rabaia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoleFeatures", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            Sql("update dbo.RoleFeatures set FeatureUniqeCode = (select FeatureUniqeCode from dbo.Features where Id = RoleFeatures.FeatureId)");
            //AlterColumn("dbo.PackageFeatures", "FeatureUniqeCode", c => c.String(nullable: true, maxLength: 120, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoleFeatures", "FeatureUniqeCode");
        }
    }
}
