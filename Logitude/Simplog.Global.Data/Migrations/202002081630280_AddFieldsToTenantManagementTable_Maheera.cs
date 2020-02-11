namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsToTenantManagementTable_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TenantManagements", "TotalNumberOfUsers", c => c.Int());
            AddColumn("dbo.TenantManagements", "TotalFreeUsers", c => c.Int());
            AddColumn("dbo.TenantManagements", "AveragePrice", c => c.Double());
            AddColumn("dbo.TenantManagements", "TotalPaymentamount", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TenantManagements", "TotalPaymentamount");
            DropColumn("dbo.TenantManagements", "AveragePrice");
            DropColumn("dbo.TenantManagements", "TotalFreeUsers");
            DropColumn("dbo.TenantManagements", "TotalNumberOfUsers");
        }
    }
}
