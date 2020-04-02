namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Wesam_AddCustomFieldsToARAndAPInvoices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.APInvoices", "Field1", c => c.String(maxLength: 250));
            AddColumn("dbo.APInvoices", "Field2", c => c.String(maxLength: 250));
            AddColumn("dbo.APInvoices", "Field3", c => c.String(maxLength: 250));
            AddColumn("dbo.APInvoices", "Field4", c => c.String(maxLength: 250));
            AddColumn("dbo.APInvoices", "Field5", c => c.String(maxLength: 250));
            AddColumn("dbo.APInvoices", "Field6", c => c.String(maxLength: 250));
            AddColumn("dbo.APInvoices", "Field7", c => c.String(maxLength: 250));
            AddColumn("dbo.APInvoices", "Field8", c => c.String(maxLength: 250));
            AddColumn("dbo.APInvoices", "Field9", c => c.String(maxLength: 250));
            AddColumn("dbo.APInvoices", "Field10", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            DropColumn("dbo.APInvoices", "Field10");
            DropColumn("dbo.APInvoices", "Field9");
            DropColumn("dbo.APInvoices", "Field8");
            DropColumn("dbo.APInvoices", "Field7");
            DropColumn("dbo.APInvoices", "Field6");
            DropColumn("dbo.APInvoices", "Field5");
            DropColumn("dbo.APInvoices", "Field4");
            DropColumn("dbo.APInvoices", "Field3");
            DropColumn("dbo.APInvoices", "Field2");
            DropColumn("dbo.APInvoices", "Field1");
        }
    }
}
