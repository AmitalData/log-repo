namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChargesTypeSearchFieldsMigration : DbMigration
    {
        public override void Up()
        {
            Sql("update ChargesTypes  set SearchFields =isnull(c.SearchFields, '') + ',' +isnull(g.DisplayNumber, '') + ',' + isnull(g.LocalName, '') + ',' from ChargesTypes as c, GLAccounts as g Where c.ReceivableCreditGLAccountId = g.Id and c.Tenant = g.Tenant update c set SearchFields = isnull(c.SearchFields, '') + ',' + isnull(g.DisplayNumber, '') + ',' + isnull(g.LocalName, '') + ',' from ChargesTypes as c, GLAccounts as g Where c.PayableDebitGLAcountId = g.Id and c.Tenant = g.Tenant");

        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipments", "FirstARInvoiceApprovalDate");
        }
    }
}
