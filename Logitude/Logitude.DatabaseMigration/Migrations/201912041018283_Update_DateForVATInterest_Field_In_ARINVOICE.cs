namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_DateForVATInterest_Field_In_ARINVOICE : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ARInvoices", "DateForInterest", c => c.DateTime());
            DropColumn("dbo.ARInvoices", "DateForVATInterest");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ARInvoices", "DateForVATInterest", c => c.DateTime());
            DropColumn("dbo.ARInvoices", "DateForInterest");
        }
    }
}
