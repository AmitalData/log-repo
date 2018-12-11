namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPaymentsGatewayFieldsAndTable_Rabaia : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaymentGatewayPartners",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        SearchFields = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            AddColumn("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode", c => c.String(maxLength: 128));
            AddColumn("dbo.TenantAdditionalDatas", "PaymentGatewayConnectionString", c => c.String());
            CreateIndex("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode");
            AddForeignKey("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode", "dbo.PaymentGatewayPartners", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode", "dbo.PaymentGatewayPartners");
            DropIndex("dbo.TenantAdditionalDatas", new[] { "PaymentGatewayPartnerCode" });
            DropColumn("dbo.TenantAdditionalDatas", "PaymentGatewayConnectionString");
            DropColumn("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode");
            DropTable("dbo.PaymentGatewayPartners");
        }
    }
}
