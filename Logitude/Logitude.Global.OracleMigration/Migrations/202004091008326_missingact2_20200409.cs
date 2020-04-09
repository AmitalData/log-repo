namespace Logitude.Global.OracleMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missingact2_20200409 : DbMigration
    {
        public override void Up()
        {
            AddColumn("TenantManagements", "BluesnapInttraStockContractId", c => c.String(nullable: true, maxLength: 15));
            AddColumn("TenantManagements", "BluesnapContractId", c => c.String(nullable: true, maxLength: 15));
        }
        
        public override void Down()
        {
        }
    }
}
