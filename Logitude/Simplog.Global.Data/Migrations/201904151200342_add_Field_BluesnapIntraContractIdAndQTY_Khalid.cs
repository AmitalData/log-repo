namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_Field_BluesnapIntraContractIdAndQTY_Khalid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TenantManagements", "BluesnapInttraStockContractId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.TenantManagements", "BluesnapInttraStockContractQTY", c => c.Int());
            CreateIndex("dbo.TenantManagements", "BluesnapInttraStockContractId");
            AddForeignKey("dbo.TenantManagements", "BluesnapInttraStockContractId", "dbo.BluesnapContracts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TenantManagements", "BluesnapInttraStockContractId", "dbo.BluesnapContracts");
            DropIndex("dbo.TenantManagements", new[] { "BluesnapInttraStockContractId" });
            DropColumn("dbo.TenantManagements", "BluesnapInttraStockContractQTY");
            DropColumn("dbo.TenantManagements", "BluesnapInttraStockContractId");
        }
    }
}
