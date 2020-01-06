namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddFeatureUniqeCode_Rabaia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Features", "FeatureUniqeCode", c => c.String(nullable: true, maxLength: 120, unicode: false));
            Sql(@"update Features set FeatureUniqeCode = ((select objecttables.Name from objecttables where id = ObjectTableId) + '.' + Code)");
            AlterColumn("dbo.Features", "FeatureUniqeCode", c => c.String(nullable: false, maxLength: 120, unicode: false));

            AddColumn("dbo.Queries", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false)); 
            AddColumn("dbo.MenuButtons", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.MenusTables", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.ObjectTableHelperControls", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.ObjectTableTabs", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));

            CreateIndex("dbo.Features", "FeatureUniqeCode", unique: true);


        }

        public override void Down()
        {
        }
    }
}
