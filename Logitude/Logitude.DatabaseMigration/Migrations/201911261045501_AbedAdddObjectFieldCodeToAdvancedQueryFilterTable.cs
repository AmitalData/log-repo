namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAdddObjectFieldCodeToAdvancedQueryFilterTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdvancedQueryFilters", "ObjectFieldCode", c => c.String(nullable: true, maxLength: 200, unicode: false));
            Sql(@"update AdvancedQueryFilters set ObjectFieldCode = (select ObjectFields.FieldCode from ObjectFields where id = AdvancedQueryFilters.ObjectFieldId)");
            AlterColumn("dbo.AdvancedQueryFilters", "ObjectFieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {

            DropColumn("dbo.AdvancedQueryFilters", "ObjectFieldCode");

        }
    }
}
