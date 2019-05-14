namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerTenantShareExportFileToTenant_Rabaia : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.States", "QBOTransactionLocationCode", c => c.String(maxLength: 3, unicode: false));
            AddColumn("dbo.Tenants", "CustomerTenantShareExportFile", c => c.Boolean(nullable: false));
            //AddColumn("dbo.ARPayments", "IsExternalEntity", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.ARPayments", "IsExternalEntity");
            DropColumn("dbo.Tenants", "CustomerTenantShareExportFile");
            //DropColumn("dbo.States", "QBOTransactionLocationCode");
        }
    }
}
