namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newcusotmsmigrations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Customs.CourierMasters", "AirlineId", "dbo.Airlines");
            CreateTable(
                "dbo.DecisionTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        LocalName = c.String(),
                        EnglishName = c.String(),
                        Inactive = c.Boolean(nullable: false),
                        SearchFields = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            AddColumn("Customs.ClaimsRelatedEntities", "DecisionCode", c => c.String(maxLength: 128));
            AddColumn("Customs.ClaimsRelatedEntities", "DecisionNote", c => c.String(maxLength: 256));
            AddColumn("Customs.ClaimsRelatedEntities", "EilatVatRefoundDecision", c => c.String(maxLength: 256));
            AddColumn("Customs.ClaimsRelatedEntities", "DepositingAmount", c => c.Decimal(precision: 16, scale: 2));
            AddColumn("Customs.ClaimsRelatedEntities", "RefundAmount", c => c.Decimal(precision: 16, scale: 2));
            AddColumn("Customs.Declarations", "CourierSuspentionCode", c => c.String(maxLength: 2, unicode: false));
            AddColumn("Customs.CourierMasters", "WeightValueCode", c => c.String(maxLength: 3, unicode: false));
            AddColumn("Customs.CustomsAirlines", "ICAO", c => c.String(maxLength: 3, unicode: false));
            AlterColumn("Customs.CustomsVendors", "VATNumber", c => c.String(maxLength: 25, unicode: false));
            CreateIndex("Customs.ClaimsRelatedEntities", "DecisionCode");
            CreateIndex("Customs.Declarations", "CourierSuspentionCode");
            CreateIndex("Customs.CourierMasters", "WeightValueCode");
            //AddForeignKey("Customs.ClaimsRelatedEntities", "DecisionCode", "dbo.DecisionTypes", "Code");
            //AddForeignKey("Customs.Declarations", "CourierSuspentionCode", "Customs.DeclarationStatusTypes", "Code");
            //AddForeignKey("Customs.CourierMasters", "AirlineId", "Customs.CustomsAirlines", "Id");
            //AddForeignKey("Customs.CourierMasters", "WeightValueCode", "Customs.FreightPaymentMethods", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.CourierMasters", "WeightValueCode", "Customs.FreightPaymentMethods");
            DropForeignKey("Customs.CourierMasters", "AirlineId", "Customs.CustomsAirlines");
            DropForeignKey("Customs.Declarations", "CourierSuspentionCode", "Customs.DeclarationStatusTypes");
            DropForeignKey("Customs.ClaimsRelatedEntities", "DecisionCode", "dbo.DecisionTypes");
            DropIndex("Customs.CourierMasters", new[] { "WeightValueCode" });
            DropIndex("Customs.Declarations", new[] { "CourierSuspentionCode" });
            DropIndex("Customs.ClaimsRelatedEntities", new[] { "DecisionCode" });
            AlterColumn("Customs.CustomsVendors", "VATNumber", c => c.String(maxLength: 15, unicode: false));
            DropColumn("Customs.CustomsAirlines", "ICAO");
            DropColumn("Customs.CourierMasters", "WeightValueCode");
            DropColumn("Customs.Declarations", "CourierSuspentionCode");
            DropColumn("Customs.ClaimsRelatedEntities", "RefundAmount");
            DropColumn("Customs.ClaimsRelatedEntities", "DepositingAmount");
            DropColumn("Customs.ClaimsRelatedEntities", "EilatVatRefoundDecision");
            DropColumn("Customs.ClaimsRelatedEntities", "DecisionNote");
            DropColumn("Customs.ClaimsRelatedEntities", "DecisionCode");
            DropTable("dbo.DecisionTypes");
            AddForeignKey("Customs.CourierMasters", "AirlineId", "dbo.Airlines", "Id");
        }
    }
}
