namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddExcelOnlyToReportTable : DbMigration
    {
        public override void Up()
        {
 
            AddColumn("dbo.Reports", "ExcelOnly", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reports", "ExcelOnly");
       
        }
    }
}
