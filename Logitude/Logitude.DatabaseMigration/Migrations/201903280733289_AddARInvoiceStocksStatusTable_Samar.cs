namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddARInvoiceStocksStatusTable_Samar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ARInvoiceStocksStatus",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 2, unicode: false),
                        Name = c.String(nullable: false, maxLength: 20, unicode: false),
                        SearchFields = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Code);
        }
        
        public override void Down()
        {
            DropTable("dbo.ARInvoiceStocksStatus");
        }
    }
}
