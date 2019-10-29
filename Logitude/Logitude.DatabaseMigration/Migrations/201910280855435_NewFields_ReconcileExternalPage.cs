namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.SqlClient;

    public partial class NewFields_ReconcileExternalPage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReconcileExternalPages", "ObjectTableId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ReconcileExternalPages", "EntityId", c => c.String(maxLength: 15, unicode: false));

            Sql("DECLARE @bankAccountTableId varchar(15) = (select Id from ObjectTables where Name = 'BankAccount');" +
                "update  r " +
                "set r.ObjectTableId = @bankAccountTableId, r.EntityId = r.BankAccountId " +
                "from ReconcileExternalPages r");

            DropForeignKey("dbo.ReconcileExternalPages", "BankAccountId", "dbo.BankAccounts");
            DropIndex("dbo.ReconcileExternalPages", new[] { "BankAccountId" });

            DropColumn("dbo.ReconcileExternalPages", "BankAccountId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReconcileExternalPages", "BankAccountId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            DropColumn("dbo.ReconcileExternalPages", "EntityId");
            DropColumn("dbo.ReconcileExternalPages", "ObjectTableId");
            CreateIndex("dbo.ReconcileExternalPages", "BankAccountId");
            AddForeignKey("dbo.ReconcileExternalPages", "BankAccountId", "dbo.BankAccounts", "Id");
        }
    }
}
