namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFactTableFieldToBIReportTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BIReports", "FactTableName", c => c.String(nullable: true, maxLength: 30));
            Sql(@"update BIReports set FactTableName='Fact_Shipments'");
            AlterColumn("dbo.BIReports", "FactTableName", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BIReports", "FactTableName");
        }
    }
}
