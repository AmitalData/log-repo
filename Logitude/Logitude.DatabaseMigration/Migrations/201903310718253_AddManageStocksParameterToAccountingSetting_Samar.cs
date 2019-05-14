namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddManageStocksParameterToAccountingSetting_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountingSettings", "EnableInvoiceStocksManagement", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountingSettings", "EnableInvoiceStocksManagement");
        }
    }
}
