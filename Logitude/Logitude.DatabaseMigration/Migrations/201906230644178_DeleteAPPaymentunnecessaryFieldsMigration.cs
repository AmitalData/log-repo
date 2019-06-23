namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteAPPaymentunnecessaryFieldsMigration : DbMigration
    {
        public override void Up()
        {



            DropColumn("dbo.APPayments", "PaymentChequeNumber");
            DropColumn("dbo.APPayments", "PaymentChequeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.APPayments", "PaymentChequeId", c => c.String());
            AddColumn("dbo.APPayments", "PaymentChequeNumber", c => c.String());
            DropForeignKey("dbo.Occasions", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Occasions", "OwnerId", "dbo.Users");
            DropForeignKey("dbo.Occasions", "OccasionTypeId", "dbo.OccasionTypes");
            DropForeignKey("dbo.OccasionTypes", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.OccasionTypes", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Occasions", "OccasionStatusId", "dbo.OccasionStatuses");
            DropForeignKey("dbo.Occasions", "IndustryId", "dbo.Industries");
            DropForeignKey("dbo.Occasions", "CreatedByUserId", "dbo.Users");
            DropIndex("dbo.OccasionTypes", new[] { "UpdatedByUserId" });
            DropIndex("dbo.OccasionTypes", new[] { "CreatedByUserId" });
            DropIndex("dbo.Occasions", new[] { "OccasionStatusId" });
            DropIndex("dbo.Occasions", new[] { "OccasionTypeId" });
            DropIndex("dbo.Occasions", new[] { "IndustryId" });
            DropIndex("dbo.Occasions", new[] { "OwnerId" });
            DropIndex("dbo.Occasions", new[] { "UpdatedByUserId" });
            DropIndex("dbo.Occasions", new[] { "CreatedByUserId" });
            DropColumn("dbo.QuoteTemplateSettings", "ShowHeaderLabelsContainers");
            DropColumn("dbo.QuoteTemplateSettings", "ShowHeaderLabelsPackages");
            DropColumn("dbo.Countries", "IsNorthAmerica");
            DropTable("dbo.OccasionTypes");
            DropTable("dbo.OccasionStatuses");
            DropTable("dbo.Occasions");
        }
    }
}
