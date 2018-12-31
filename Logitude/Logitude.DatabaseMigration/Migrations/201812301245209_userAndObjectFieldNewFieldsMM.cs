namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userAndObjectFieldNewFieldsMM : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ShowLocalNameInLOV", c => c.Boolean(nullable: false));
            AddColumn("dbo.ObjectFields", "DisplayOnLookUpLocal", c => c.Boolean(nullable: false));
            //AddColumn("dbo.APInvoices", "FirstApproveDate", c => c.DateTime());
            //AddColumn("dbo.APPayments", "FirstApproveDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.APPayments", "FirstApproveDate");
            //DropColumn("dbo.APInvoices", "FirstApproveDate");
            DropColumn("dbo.ObjectFields", "DisplayOnLookUpLocal");
            DropColumn("dbo.Users", "ShowLocalNameInLOV");
        }
    }
}
