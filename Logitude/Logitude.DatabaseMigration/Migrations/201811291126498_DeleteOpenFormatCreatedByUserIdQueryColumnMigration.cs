namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteOpenFormatCreatedByUserIdQueryColumnMigration : DbMigration
    {
        public override void Up()
        {
            Sql("delete from querycolumns where objectfieldid=(select id from objectfields where fieldname='CreatedbyuserId' and objecttableid=(select id from objecttables where name='openformatreport'))");
        }
        
        public override void Down()
        {
            DropColumn("dbo.BatchTaskExecutions", "CallStack");
        }
    }
}
