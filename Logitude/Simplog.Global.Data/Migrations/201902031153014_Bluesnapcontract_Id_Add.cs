namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bluesnapcontract_Id_Add : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TenantManagements", name: "BluesnapContractCode", newName: "BluesnapContractId");
            RenameIndex(table: "dbo.TenantManagements", name: "IX_BluesnapContractCode", newName: "IX_BluesnapContractId");
            AddColumn("dbo.BluesnapContracts", "Id", c => c.String(nullable: false, maxLength: 15, unicode: false));
            Sql("declare @ID as varchar(15)" +
                " declare @Code as varchar(15)" +
                " BEGIN" +
                " DECLARE Bluesnapcontract CURSOR READ_ONLY" +
                " FOR " +
                "SELECT Code" +
                " FROM Bluesnapcontracts " +
                "OPEN Bluesnapcontract FETCH NEXT FROM Bluesnapcontract INTO @Code" +
                " WHILE @@FETCH_STATUS = 0" +
                " BEGIN " +
                "if  exists(select Code from Bluesnapcontracts where Code = @Code)" +
                " begin" +
                " update Bluesnapcontracts set Id = @Code where Code = @Code " +
                "end" +
                " FETCH NEXT FROM Bluesnapcontract INTO @Code" +
                " END " +
                "CLOSE Bluesnapcontract" +
                " DEALLOCATE Bluesnapcontract " +
                "END");
        }
        
        public override void Down()
        {
            DropColumn("dbo.BluesnapContracts", "Id");
            RenameIndex(table: "dbo.TenantManagements", name: "IX_BluesnapContractId", newName: "IX_BluesnapContractCode");
            RenameColumn(table: "dbo.TenantManagements", name: "BluesnapContractId", newName: "BluesnapContractCode");
        }
    }
}
