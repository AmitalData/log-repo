namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BluesnapcontractId_Key_TenantManagement_RemoveForeignKey : DbMigration
    {
        public override void Up()
        {
            Sql("declare @ID as varchar(15) declare @Code as varchar(15) BEGIN DECLARE Bluesnapcontract CURSOR READ_ONLY FOR SELECT Code, Id FROM Bluesnapcontracts OPEN Bluesnapcontract FETCH NEXT FROM Bluesnapcontract INTO @Code, @Id WHILE @@FETCH_STATUS = 0 BEGIN if  exists(select BluesnapContractId from TenantManagements where BluesnapContractId = @Code) begin print(@Code) update TenantManagements set BluesnapContractId = @Id where BluesnapContractId = @Code end FETCH NEXT FROM Bluesnapcontract INTO @Code ,@Id END CLOSE Bluesnapcontract DEALLOCATE Bluesnapcontract END");
              DropForeignKey("dbo.TenantManagements", "BluesnapContractId", "dbo.BluesnapContracts");
              DropIndex("dbo.TenantManagements", new[] { "BluesnapContractId" });
            AlterColumn("dbo.TenantManagements", "BluesnapContractId", c => c.String(maxLength: 15, unicode: false));
            DropPrimaryKey("dbo.BluesnapContracts");
            AddPrimaryKey("dbo.BluesnapContracts", "Id");
            CreateIndex("dbo.TenantManagements", "BluesnapContractId");
            AddForeignKey("dbo.TenantManagements", "BluesnapContractId", "dbo.BluesnapContracts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TenantManagements", "BluesnapContractId", "dbo.BluesnapContracts");
            DropIndex("dbo.TenantManagements", new[] { "BluesnapContractId" });
            DropPrimaryKey("dbo.BluesnapContracts");
            AddPrimaryKey("dbo.BluesnapContracts", "Code");
        }
    }
}
