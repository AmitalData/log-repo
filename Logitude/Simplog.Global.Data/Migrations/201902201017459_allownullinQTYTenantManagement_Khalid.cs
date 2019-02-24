namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class allownullinQTYTenantManagement_Khalid : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TenantManagements", "BluesnapContractQTY", c => c.Int());
            AlterColumn("dbo.TenantManagements", "BluesnapCRMContractQTY", c => c.Int());
            AlterColumn("dbo.TenantManagements", "BluesnapEAWBContractQTY", c => c.Int());
            AlterColumn("dbo.TenantManagements", "BluesnapEAWBSContractQTY", c => c.Int());
            AlterColumn("dbo.TenantManagements", "BluesnapOneTimeContractQTY", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TenantManagements", "BluesnapOneTimeContractQTY", c => c.Int(nullable: false));
            AlterColumn("dbo.TenantManagements", "BluesnapEAWBSContractQTY", c => c.Int(nullable: false));
            AlterColumn("dbo.TenantManagements", "BluesnapEAWBContractQTY", c => c.Int(nullable: false));
            AlterColumn("dbo.TenantManagements", "BluesnapCRMContractQTY", c => c.Int(nullable: false));
            AlterColumn("dbo.TenantManagements", "BluesnapContractQTY", c => c.Int(nullable: false));
        }
    }
}
