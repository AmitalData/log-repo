namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddNewFieldTenantEmailSendingQuotaToTenantTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tenants", "TenantEmailSendingQuota", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tenants", "TenantEmailSendingQuota");
        }
    }
}
