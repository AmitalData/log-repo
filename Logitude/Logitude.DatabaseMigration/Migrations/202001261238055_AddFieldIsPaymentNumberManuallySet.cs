namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldIsPaymentNumberManuallySet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ARPayments", "IsPaymentNumberManuallySet", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ARPayments", "IsPaymentNumberManuallySet");
        }
    }
}
