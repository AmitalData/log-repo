namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFirstApproveDate : DbMigration
    {
        public override void Up()
        {           
            AddColumn("dbo.APInvoices", "FirstApproveDate", c => c.DateTime());
            AddColumn("dbo.APPayments", "FirstApproveDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.APPayments", "FirstApproveDate");
            DropColumn("dbo.APInvoices", "FirstApproveDate");
        }
    }
}
