namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPackageCodeSearchFieldInTenantManagement_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TenantManagements", "PackageCodeSearchField", c => c.String(maxLength: 250));

            Sql(
                @"
                delete from AdvancedQueryFilters 
                where
                IsPredefined = 0 
                and QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'TenantManagement') )
                and ObjectFieldId = (select Id from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'TenantManagement') and FieldName = 'PackageCode')
                ");
        }
        
        public override void Down()
        {
            DropColumn("dbo.TenantManagements", "PackageCodeSearchField");
        }
    }
}
