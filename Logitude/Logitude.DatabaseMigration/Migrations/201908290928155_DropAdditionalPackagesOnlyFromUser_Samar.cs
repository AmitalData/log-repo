namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropAdditionalPackagesOnlyFromUser_Samar : DbMigration
    {
        public override void Up()
        {
            Sql(@"delete from AdvancedQueryFilters where ObjectFieldId = (select Id from ObjectFields where FieldName = 'AdditionalPackagesOnly')
                delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'AdditionalPackagesOnly')
                delete from ObjectFields where FieldName = 'AdditionalPackagesOnly'
                delete from TextCodes where Code like '%AdditionalPackagesOnly%'");
            
            DropColumn("dbo.Users", "AdditionalPackagesOnly");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "AdditionalPackagesOnly", c => c.Boolean(nullable: false));
        }
    }
}
