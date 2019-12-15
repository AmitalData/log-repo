namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItrestTransactionTableConstraintMigration : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE InterestTransactions  ADD  CONSTRAINT [UQ_InterestEntityTypeCode_EntityId_Tenant_OriginalEntityLineNumber] UNIQUE NONCLUSTERED ([Tenant] ASC,[InterestEntityTypeCode] ASC,[EntityId] ASC,[OriginalEntityLineNumber] ASC)");
        }
        
        public override void Down()
        {
        }
    }
}
