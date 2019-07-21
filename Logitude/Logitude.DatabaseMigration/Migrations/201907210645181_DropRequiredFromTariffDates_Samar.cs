namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropRequiredFromTariffDates_Samar : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tariffs", "StartDate", c => c.DateTime());
            AlterColumn("dbo.Tariffs", "ExpirationDate", c => c.DateTime());

            Sql("delete from QueryColumns where ObjectFieldId in (select Id from ObjectFields where FieldName = 'StartDate') and QueryId in (select Id from Queries where Code = 'Air Surcharges Cost Tariffs')");
            Sql("delete from QueryColumns where ObjectFieldId in (select Id from ObjectFields where FieldName = 'ExpirationDate') and QueryId in (select Id from Queries where Code = 'Air Surcharges Cost Tariffs')");
        }
        
        public override void Down()
        {            
            AlterColumn("dbo.Tariffs", "ExpirationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Tariffs", "StartDate", c => c.DateTime(nullable: false));            
        }
    }
}
