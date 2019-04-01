namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddARInvoiceStockLineTable_Samar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ARInvoiceStockLines",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        ARInvoiceStockId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Number = c.String(nullable: false, maxLength: 20, unicode: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        IsUsed = c.Boolean(nullable: false),
                        ARInvoiceId = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ARInvoices", t => t.ARInvoiceId)
                .ForeignKey("dbo.ARInvoiceStocks", t => t.ARInvoiceStockId)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.ARInvoiceStockId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.ARInvoiceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ARInvoiceStockLines", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.ARInvoiceStockLines", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.ARInvoiceStockLines", "ARInvoiceStockId", "dbo.ARInvoiceStocks");
            DropForeignKey("dbo.ARInvoiceStockLines", "ARInvoiceId", "dbo.ARInvoices");
            DropIndex("dbo.ARInvoiceStockLines", new[] { "ARInvoiceId" });
            DropIndex("dbo.ARInvoiceStockLines", new[] { "UpdatedByUserId" });
            DropIndex("dbo.ARInvoiceStockLines", new[] { "CreatedByUserId" });
            DropIndex("dbo.ARInvoiceStockLines", new[] { "ARInvoiceStockId" });
            DropTable("dbo.ARInvoiceStockLines");
        }
    }
}
