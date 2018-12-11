namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLOVAdditionalColumnsToDWObjectField_Rabaia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DWObjectFields", "LOVAdditionalColumns", c => c.String(maxLength: 1000, unicode: false));
            //AlterColumn("dbo.OpenFormatReports", "FromDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.OpenFormatReports", "ToDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.OpenFormatReports", "ToDate", c => c.DateTime());
            //AlterColumn("dbo.OpenFormatReports", "FromDate", c => c.DateTime());
            DropColumn("dbo.DWObjectFields", "LOVAdditionalColumns");
        }
    }
}
