namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Abed_AddPaymentGatewayPartnerToLogitudeContext_PocoAndMap : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode", "dbo.PaymentGatewayPartners");
            DropIndex("dbo.TenantAdditionalDatas", new[] { "PaymentGatewayPartnerCode" });
            DropPrimaryKey("dbo.PaymentGatewayPartners");
            AlterColumn("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.PaymentGatewayPartners", "Code", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.PaymentGatewayPartners", "Name", c => c.String(nullable: false, maxLength: 150, unicode: false));
            AlterColumn("dbo.PaymentGatewayPartners", "SearchFields", c => c.String(maxLength: 1000));
            AddPrimaryKey("dbo.PaymentGatewayPartners", "Code");
            CreateIndex("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode");
            AddForeignKey("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode", "dbo.PaymentGatewayPartners", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode", "dbo.PaymentGatewayPartners");
            DropIndex("dbo.TenantAdditionalDatas", new[] { "PaymentGatewayPartnerCode" });
            DropPrimaryKey("dbo.PaymentGatewayPartners");
            AlterColumn("dbo.PaymentGatewayPartners", "SearchFields", c => c.String());
            AlterColumn("dbo.PaymentGatewayPartners", "Name", c => c.String());
            AlterColumn("dbo.PaymentGatewayPartners", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.PaymentGatewayPartners", "Code");
            CreateIndex("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode");
            AddForeignKey("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode", "dbo.PaymentGatewayPartners", "Code");
        }
    }
}
