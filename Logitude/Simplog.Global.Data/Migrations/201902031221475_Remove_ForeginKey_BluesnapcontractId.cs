namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_ForeginKey_BluesnapcontractId : DbMigration
    {
        public override void Up()
        {
            Sql("IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.TenantManagements_dbo.BluesnapContracts_BluesnapContractCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[TenantManagements]')) ALTER TABLE[dbo].[TenantManagements] DROP CONSTRAINT[FK_dbo.TenantManagements_dbo.BluesnapContracts_BluesnapContractCode] ");           
            AlterColumn("dbo.TenantManagements", "BluesnapContractId", c => c.String(maxLength: 10, unicode: false));
            DropForeignKey("dbo.TenantManagements", "BluesnapContractId", "dbo.BluesnapContracts");
           DropIndex("dbo.TenantManagements", new[] { "BluesnapContractId" });

            AddForeignKey("dbo.TenantManagements", "BluesnapContractId", "dbo.BluesnapContracts");
            Sql("declare @ID as varchar(15) declare @Code as varchar(15) BEGIN DECLARE Bluesnapcontract CURSOR READ_ONLY FOR SELECT Code, Id FROM Bluesnapcontracts OPEN Bluesnapcontract FETCH NEXT FROM Bluesnapcontract INTO @Code, @Id WHILE @@FETCH_STATUS = 0 BEGIN if  exists(select BluesnapContractId from TenantManagements where BluesnapContractId = @Code) begin print(@Code) update TenantManagements set BluesnapContractId = @Id where BluesnapContractId = @Code end FETCH NEXT FROM Bluesnapcontract INTO @Code ,@Id END CLOSE Bluesnapcontract DEALLOCATE Bluesnapcontract END");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TenantManagements", "BluesnapContractId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.TenantManagements", "BluesnapContractId");
            AddForeignKey("dbo.TenantManagements", "BluesnapContractId", "dbo.BluesnapContracts", "Code");
        }
    }
}
