namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCardContactProduct_Samar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CardContactProducts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CardContactId = c.String(nullable: false, maxLength: 15, unicode: false),
                        ProductTypeCode = c.String(nullable: false, maxLength: 2, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CardContacts", t => t.CardContactId)
                .ForeignKey("dbo.ProductTypes", t => t.ProductTypeCode)
                .Index(t => t.CardContactId)
                .Index(t => t.ProductTypeCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CardContactProducts", "ProductTypeCode", "dbo.ProductTypes");
            DropForeignKey("dbo.CardContactProducts", "CardContactId", "dbo.CardContacts");
            DropIndex("dbo.CardContactProducts", new[] { "ProductTypeCode" });
            DropIndex("dbo.CardContactProducts", new[] { "CardContactId" });
            DropTable("dbo.CardContactProducts");
        }
    }
}
