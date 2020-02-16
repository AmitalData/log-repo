namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InterestTransactionMigration : DbMigration
    {
        public override void Up()
        {
          //  Sql("ALTER TABLE InterestTransactions drop  CONSTRAINT[UQ_InterestEntityTypeCode_EntityId_Tenant_OriginalEntityLineNumber]");
        //    Sql("ALTER TABLE InterestTransactions add  CONSTRAINT[UQ_InterestEntityTypeCode_EntityId_Tenant_OriginalEntityLineNumber_GLAccountId] UNIQUE NONCLUSTERED([Tenant] ASC, [InterestEntityTypeCode] ASC, [EntityId] ASC, [OriginalEntityLineNumber] ASC, [GLAccountId] ASC)");

        }

        public override void Down()
        {
            DropColumn("dbo.WarehouseReleases", "IsUsed");
            DropColumn("dbo.GLAccounts", "Smallcashbook");
            DropColumn("dbo.ShipmentComputedFields", "OperationallyClosedByUserName");
        }
    }
}
