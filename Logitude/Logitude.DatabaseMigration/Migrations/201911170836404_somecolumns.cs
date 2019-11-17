namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class somecolumns : DbMigration
    {
        public override void Up()
        {
           
            AddColumn("dbo.APPayments", "VendorBankAddress", c => c.String(maxLength: 100));
            AddColumn("dbo.APPayments", "VendorBankName", c => c.String(maxLength: 40));
            AddColumn("dbo.APPayments", "VendorBankAccountNumber", c => c.String(maxLength: 25, unicode: false));
            AddColumn("dbo.APPayments", "VendorSwift", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.APPayments", "VendorIBANNumber", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.APPayments", "VendorIBANNumber");
            DropColumn("dbo.APPayments", "VendorSwift");
            DropColumn("dbo.APPayments", "VendorBankAccountNumber");
            DropColumn("dbo.APPayments", "VendorBankName");
            DropColumn("dbo.APPayments", "VendorBankAddress");
          
        }
    }
}
