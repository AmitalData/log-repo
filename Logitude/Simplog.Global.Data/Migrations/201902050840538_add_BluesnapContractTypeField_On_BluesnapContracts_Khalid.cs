namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_BluesnapContractTypeField_On_BluesnapContracts_Khalid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BluesnapContracts", "BluesnapContractTypeCode", c => c.String(nullable: false, maxLength: 4, unicode: false,defaultValue: "BA"));
            CreateIndex("dbo.BluesnapContracts", "BluesnapContractTypeCode");
            AddForeignKey("dbo.BluesnapContracts", "BluesnapContractTypeCode", "dbo.BluesnapContractTypes", "Code", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BluesnapContracts", "BluesnapContractTypeCode", "dbo.BluesnapContractTypes");
            DropIndex("dbo.BluesnapContracts", new[] { "BluesnapContractTypeCode" });
            DropColumn("dbo.BluesnapContracts", "BluesnapContractTypeCode");
        }
    }
}
