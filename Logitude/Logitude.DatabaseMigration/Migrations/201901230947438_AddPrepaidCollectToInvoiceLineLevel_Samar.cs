namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPrepaidCollectToInvoiceLineLevel_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.APInvoiceLines", "PrepaidCollectId", c => c.String(maxLength: 1, unicode: false));
            AddColumn("dbo.ARInvoiceLines", "PrepaidCollectId", c => c.String(maxLength: 1, unicode: false));
            CreateIndex("dbo.APInvoiceLines", "PrepaidCollectId");
            CreateIndex("dbo.ARInvoiceLines", "PrepaidCollectId");
            AddForeignKey("dbo.APInvoiceLines", "PrepaidCollectId", "dbo.PrepaidCollects", "Id");
            AddForeignKey("dbo.ARInvoiceLines", "PrepaidCollectId", "dbo.PrepaidCollects", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ARInvoiceLines", "PrepaidCollectId", "dbo.PrepaidCollects");
            DropForeignKey("dbo.APInvoiceLines", "PrepaidCollectId", "dbo.PrepaidCollects");
            DropIndex("dbo.ARInvoiceLines", new[] { "PrepaidCollectId" });
            DropIndex("dbo.APInvoiceLines", new[] { "PrepaidCollectId" });
            DropColumn("dbo.ARInvoiceLines", "PrepaidCollectId");
            DropColumn("dbo.APInvoiceLines", "PrepaidCollectId");
        }
    }
}
