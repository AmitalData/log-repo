namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_Alter_UniqueQueryCode : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AdvancedQueryFilters", "QueryCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
            //AlterColumn("dbo.Queries", "UniqueCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AlterColumn("dbo.Queries", "OriginalQueryCode", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.QueryColumns", "QueryCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AlterColumn("dbo.SharedUserQueries", "QueryCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SharedUserQueries", "QueryCode", c => c.String(nullable: false, maxLength: 30, unicode: false));
            AlterColumn("dbo.QueryColumns", "QueryCode", c => c.String(nullable: false, maxLength: 30, unicode: false));
            AlterColumn("dbo.Queries", "OriginalQueryCode", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.Queries", "UniqueCode", c => c.String(nullable: false, maxLength: 30, unicode: false));
            AlterColumn("dbo.AdvancedQueryFilters", "QueryCode", c => c.String(nullable: false, maxLength: 30, unicode: false));
        }
    }
}
