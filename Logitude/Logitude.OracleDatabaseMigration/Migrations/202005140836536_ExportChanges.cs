namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExportChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Customs.SuppInvoiceItemsAbachStatement", "StatementType", c => c.String(maxLength: 4, unicode: false));
            CreateIndex("Customs.SuppInvoiceItemsAbachStatement", "StatementType");
            AddForeignKey("Customs.SuppInvoiceItemsAbachStatement", "StatementType", "Customs.NbcDeclarationTypes", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.SuppInvoiceItemsAbachStatement", "StatementType", "Customs.NbcDeclarationTypes");
            DropIndex("Customs.SuppInvoiceItemsAbachStatement", new[] { "StatementType" });
            AlterColumn("Customs.SuppInvoiceItemsAbachStatement", "StatementType", c => c.String(maxLength: 3));
        }
    }
}
