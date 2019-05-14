namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddApprovedateFieldsOnARPaymentTable_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ARPayments", "ApprovedDate", c => c.DateTime());
            AddColumn("dbo.ARPayments", "ApprovedByUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ARPayments", "FirstApproveDate", c => c.DateTime());
            CreateIndex("dbo.ARPayments", "ApprovedByUserId");
            AddForeignKey("dbo.ARPayments", "ApprovedByUserId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ARPayments", "ApprovedByUserId", "dbo.Users");
            DropIndex("dbo.ARPayments", new[] { "ApprovedByUserId" });
            DropColumn("dbo.ARPayments", "FirstApproveDate");
            DropColumn("dbo.ARPayments", "ApprovedByUserId");
            DropColumn("dbo.ARPayments", "ApprovedDate");
        }
    }
}
