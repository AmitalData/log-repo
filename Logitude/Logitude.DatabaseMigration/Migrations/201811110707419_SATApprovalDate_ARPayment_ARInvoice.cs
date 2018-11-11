namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SATApprovalDate_ARPayment_ARInvoice : DbMigration
    {
        public override void Up()
        {
          
            AddColumn("dbo.ARInvoices", "SATApprovalDate", c => c.DateTime());
            AddColumn("dbo.ARPayments", "SATApprovalDate", c => c.DateTime());
            
        }
        
        public override void Down()
        {
          
            DropColumn("dbo.ARPayments", "SATApprovalDate");
            DropColumn("dbo.ARInvoices", "SATApprovalDate");
            
        }
    }
}
