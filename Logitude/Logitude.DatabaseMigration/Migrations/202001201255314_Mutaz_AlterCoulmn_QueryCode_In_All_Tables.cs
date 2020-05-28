namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_AlterCoulmn_QueryCode_In_All_Tables : DbMigration
    {
        public override void Up()
        {
              //AlterColumn("dbo.Queries", "UniqeCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
              AlterColumn("dbo.SharedUserQueries", "QueryCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
              AlterColumn("dbo.Queries", "OriginalQueryCode", c => c.String(maxLength: 200, unicode: false));
              AlterColumn("dbo.QueryColumns", "QueryCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
              AlterColumn("dbo.AdvancedQueryFilters", "QueryCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }

        public override void Down()
        {
        }
    }
}
