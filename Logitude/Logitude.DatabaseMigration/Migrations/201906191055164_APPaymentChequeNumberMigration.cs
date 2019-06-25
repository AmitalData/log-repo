namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APPaymentChequeNumberMigration : DbMigration
    {
        public override void Up()
        {
          
            AddColumn("dbo.APPayments", "PaymentChequeNumber", c => c.String());
         
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductTypeModifications", "QuotationDefaultTemplateId", c => c.String());
            DropColumn("dbo.ProductTypeModifications", "RoutingRQuoteDefaultTemplateId");
            DropColumn("dbo.APPayments", "PaymentChequeNumber");
            RenameIndex(table: "dbo.ProductTypes", name: "IX_RoutingRQuoteDefaultTemplateId", newName: "IX_RatesQuotationDefaultTemplateId");
            RenameColumn(table: "dbo.ProductTypes", name: "RoutingRQuoteDefaultTemplateId", newName: "RatesQuotationDefaultTemplateId");
        }
    }
}
