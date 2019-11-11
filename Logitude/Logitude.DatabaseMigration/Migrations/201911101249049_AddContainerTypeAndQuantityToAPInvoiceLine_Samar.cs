namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContainerTypeAndQuantityToAPInvoiceLine_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.APInvoiceLines", "ContainerTypeId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.APInvoiceLines", "Quantity", c => c.Int());
            CreateIndex("dbo.APInvoiceLines", "ContainerTypeId");
            AddForeignKey("dbo.APInvoiceLines", "ContainerTypeId", "dbo.PackageTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.APInvoiceLines", "ContainerTypeId", "dbo.PackageTypes");
            DropIndex("dbo.APInvoiceLines", new[] { "ContainerTypeId" });
            DropColumn("dbo.APInvoiceLines", "Quantity");
            DropColumn("dbo.APInvoiceLines", "ContainerTypeId");
        }
    }
}
