namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_Add_Field_QueryCode_To_AdvanceQuery_And_Delete_ForienKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdvancedQueryFilters", "QueryCode", c => c.String(nullable: true, maxLength: 30, unicode: false));
            Sql(@"update AdvancedQueryFilters set QueryCode = (select Queries.Code from Queries where Id = AdvancedQueryFilters.QueryId)");
            AlterColumn("dbo.AdvancedQueryFilters", "QueryCode", c => c.String(nullable: false, maxLength: 30, unicode: false));
            Sql("ALTER TABLE AdvancedQueryFilters DROP CONSTRAINT  FK_QueryAdvancedQueryFilter");
        }
        
        public override void Down()
        {
             DropColumn("dbo.AdvancedQueryFilters", "QueryCode");
        }
    }
}
