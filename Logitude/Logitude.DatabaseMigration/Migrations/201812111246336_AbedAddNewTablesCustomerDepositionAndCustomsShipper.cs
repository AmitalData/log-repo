namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddNewTablesCustomerDepositionAndCustomsShipper : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerDepositions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CustomsShipperId = c.String(nullable: false, maxLength: 15, unicode: false),
                        DepositionNumber = c.String(nullable: false, maxLength: 20, unicode: false),
                        ValidityStartDate = c.DateTime(),
                        ValidityEndDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomsShippers", t => t.CustomsShipperId)
                .Index(t => t.CustomsShipperId);
            
            CreateTable(
                "dbo.CustomsShippers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CustomsShipperCode = c.String(nullable: false, maxLength: 15, unicode: false),
                        ValidDepositionNumber = c.String(nullable: false, maxLength: 20, unicode: false),
                        ValidityStartDate = c.DateTime(),
                        ValidityEndDate = c.DateTime(),
                        FutureDepositionExist = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerDepositions", "CustomsShipperId", "dbo.CustomsShippers");
            DropIndex("dbo.CustomerDepositions", new[] { "CustomsShipperId" });

            DropTable("dbo.CustomsShippers");
            DropTable("dbo.CustomerDepositions");
        }
    }
}
