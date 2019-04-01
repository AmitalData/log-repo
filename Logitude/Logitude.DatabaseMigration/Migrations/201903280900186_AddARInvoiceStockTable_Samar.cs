namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddARInvoiceStockTable_Samar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ARInvoiceStocks",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 40, unicode: false),
                        Description = c.String(maxLength: 250, unicode: false),
                        Inactive = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        StatusCode = c.String(nullable: false, maxLength: 2, unicode: false),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        Amount = c.Int(),
                        Remaining = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.ARInvoiceStocksStatus", t => t.StatusCode)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.StatusCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ARInvoiceStocks", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.ARInvoiceStocks", "StatusCode", "dbo.ARInvoiceStocksStatus");
            DropForeignKey("dbo.ARInvoiceStocks", "CreatedByUserId", "dbo.Users");
            DropIndex("dbo.ARInvoiceStocks", new[] { "StatusCode" });
            DropIndex("dbo.ARInvoiceStocks", new[] { "UpdatedByUserId" });
            DropIndex("dbo.ARInvoiceStocks", new[] { "CreatedByUserId" });
            DropTable("dbo.ARInvoiceStocks");
        }
    }
}
