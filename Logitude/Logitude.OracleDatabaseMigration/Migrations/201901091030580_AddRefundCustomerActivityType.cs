namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRefundCustomerActivityType : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Customs.Declarations", "CourierSuspentionCode", "Customs.DeclarationStatusTypes");
            DropIndex("Customs.Declarations", new[] { "CourierSuspentionCode" });
            CreateTable(
                "Customs.RefundCustomerActivityTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 2, unicode: false),
                        EnglishName = c.String(maxLength: 40, unicode: false),
                        SearchFields = c.String(maxLength: 1000),
                        LocalName = c.String(maxLength: 40),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            DropColumn("Customs.Declarations", "CourierSuspentionCode");
        }
        
        public override void Down()
        {
            AddColumn("Customs.Declarations", "CourierSuspentionCode", c => c.String(maxLength: 2, unicode: false));
            DropTable("Customs.RefundCustomerActivityTypes");
            CreateIndex("Customs.Declarations", "CourierSuspentionCode");
            AddForeignKey("Customs.Declarations", "CourierSuspentionCode", "Customs.DeclarationStatusTypes", "Code");
        }
    }
}
