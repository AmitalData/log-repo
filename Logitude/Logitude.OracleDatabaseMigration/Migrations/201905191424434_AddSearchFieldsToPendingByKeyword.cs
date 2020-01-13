namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSearchFieldsToPendingByKeyword : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Activities", name: "From", newName: "FromC");
            RenameColumn(table: "dbo.Activities", name: "To", newName: "ToC");
            AddColumn("Customs.PendingByKeywords", "SearchFields", c => c.String(maxLength: 2000));
            AlterColumn("dbo.Activities", "ToC", c => c.String(maxLength: 500, unicode: false));
            AlterColumn("dbo.APInvoices", "SearchFields", c => c.String(maxLength: 2000));
            AlterColumn("dbo.Shipments", "SearchFields", c => c.String(maxLength: 2000));
            AlterColumn("dbo.ReportsTemplates", "CC", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ReportsTemplates", "CC", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Shipments", "SearchFields", c => c.String(maxLength: 4000));
            AlterColumn("dbo.APInvoices", "SearchFields", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Activities", "ToC", c => c.String(maxLength: 4000, unicode: false));
            DropColumn("Customs.PendingByKeywords", "SearchFields");
            RenameColumn(table: "dbo.Activities", name: "ToC", newName: "To");
            RenameColumn(table: "dbo.Activities", name: "FromC", newName: "From");
        }
    }
}
