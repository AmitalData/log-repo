namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewField_SearchFields_AccountingIntegirityCheck : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountingIntegrityChecks", "SearchFields", c => c.String(maxLength: 4000));
            //AlterColumn("dbo.Shipments", "Field1", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field2", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field3", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field4", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field5", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field6", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field7", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field8", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field9", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field10", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field11", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field12", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field13", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field14", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field15", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field16", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field17", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field18", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field19", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field20", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field21", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field22", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field23", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field24", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field25", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field26", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field27", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field28", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field29", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field30", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field31", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field32", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field33", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field34", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field35", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field36", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field37", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field38", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field39", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.Shipments", "Field40", c => c.String(maxLength: 2000));
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.Shipments", "Field40", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field39", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field38", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field37", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field36", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field35", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field34", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field33", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field32", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field31", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field30", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field29", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field28", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field27", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field26", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field25", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field24", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field23", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field22", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field21", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field20", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field19", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field18", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field17", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field16", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field15", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field14", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field13", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field12", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field11", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field10", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field9", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field8", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field7", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field6", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field5", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field4", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field3", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field2", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Shipments", "Field1", c => c.String(maxLength: 250));
            DropColumn("dbo.AccountingIntegrityChecks", "SearchFields");
        }
    }
}
