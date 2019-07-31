namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewField_IsFullAccounting_ARPayment : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.AccountingSettings", "IsARInvoiceChronologicalDates", c => c.Boolean(nullable: false));
            //AddColumn("dbo.AccountingSettings", "IsARPaymentChronologicalDates", c => c.Boolean(nullable: false));
            //AddColumn("dbo.ARPayments", "ApprovedDate", c => c.DateTime());
            //AddColumn("dbo.ARPayments", "ApprovedByUserId", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.ARPayments", "FirstApproveDate", c => c.DateTime());
            AddColumn("dbo.ARPayments", "IsFullAccounting", c => c.Boolean(nullable: false));
            //CreateIndex("dbo.ARPayments", "ApprovedByUserId");
            //AddForeignKey("dbo.ARPayments", "ApprovedByUserId", "dbo.Users", "Id");
            //DropColumn("dbo.AccountingSettings", "IsChronologicalDates");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.AccountingSettings", "IsChronologicalDates", c => c.Boolean(nullable: false));
            //DropForeignKey("dbo.ARPayments", "ApprovedByUserId", "dbo.Users");
            //DropIndex("dbo.ARPayments", new[] { "ApprovedByUserId" });
            DropColumn("dbo.ARPayments", "IsFullAccounting");
            //DropColumn("dbo.ARPayments", "FirstApproveDate");
            //DropColumn("dbo.ARPayments", "ApprovedByUserId");
            //DropColumn("dbo.ARPayments", "ApprovedDate");
            //DropColumn("dbo.AccountingSettings", "IsARPaymentChronologicalDates");
            //DropColumn("dbo.AccountingSettings", "IsARInvoiceChronologicalDates");
        }
    }
}
