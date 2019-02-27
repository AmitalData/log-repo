namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TenantManagement_BluesnapOnetimeContract_Field_Change : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TenantManagements", "BluesnapOneTimeContractId", "dbo.BluesnapContracts");
            DropIndex("dbo.TenantManagements", new[] { "BluesnapOneTimeContractId" });
            RenameColumn(table: "dbo.TenantManagements", name: "BluesnapOneTimeContractId", newName: "BluesnapOneTimeContract");
            AlterColumn("dbo.TenantManagements", "BluesnapOneTimeContract", c => c.String(maxLength: 40));
        }

        public override void Down()
        {
            RenameColumn(table: "dbo.TenantManagements", name: "BluesnapOneTimeContract", newName: "BluesnapOneTimeContractId");
            CreateIndex("dbo.TenantManagements", "BluesnapOneTimeContractId");
            AddForeignKey("dbo.TenantManagements", "BluesnapOneTimeContractId", "dbo.BluesnapContracts", "Id");
            AlterColumn("dbo.TenantManagements", "BluesnapOneTimeContract", c => c.String(maxLength: 15));
        }
    }
}
