namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missingmaindbmigrations : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AWBMessagingStocks", newName: "MessagingStocks");
            RenameTable(name: "dbo.AWBStockUsageHistories", newName: "MessagingStockUsageHistories");
            DropForeignKey("dbo.Journals", "TaxReportStatusCode", "dbo.TaxReportStatus");
            DropIndex("dbo.Journals", new[] { "TaxReportStatusCode" });
            CreateTable(
                "dbo.CustomsShippers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CustomsShipperCode = c.String(nullable: false, maxLength: 15, unicode: false),
                        ValidDepositionNumber = c.String(nullable: false, maxLength: 20, unicode: false),
                        ValidityStartDate = c.DateTime(precision: 7),
                        ValidityEndDate = c.DateTime(precision: 7),
                        SearchFields = c.String(maxLength: 1000, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cards", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.PaymentGatewayPartners",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        SearchFields = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            AddColumn("dbo.Tenants", "LocalAddressId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tenants", "TenantEmailSendingQuota", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "ShowLocalNameInLOV", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "ShowSaleMaxMinAmountPackages", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "ShowSaleMaxMinAmountContainers", c => c.Boolean(nullable: false));
            AddColumn("dbo.Customers", "ActivationDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.Customers", "InactiveDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.Customers", "ActivationRequestDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.Customers", "ActivatedByUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Customers", "SetAsInactiveByUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Customers", "ActivationRequestedByUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tickets", "EntityType", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ObjectFields", "DisplayOnLookUpLocal", c => c.Boolean(nullable: false));
            AddColumn("dbo.Queries", "SharedWithAll", c => c.Boolean(nullable: false));
            AddColumn("dbo.Queries", "SharedWithSpecificUsers", c => c.Boolean(nullable: false));
            AddColumn("dbo.Queries", "SharedByUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Queries", "SpotlightModeActivated", c => c.Boolean(nullable: false));
            AddColumn("dbo.AgentSharedManifests", "CancelledBySenderAgent", c => c.Boolean(nullable: false));
            AddColumn("dbo.APInvoices", "FirstApproveDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.APPayments", "FirstApproveDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.ARInvoices", "SATApprovalDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.ARPayments", "SATApprovalDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.MessagingStocks", "StockType", c => c.String(maxLength: 10, unicode: false));
            AddColumn("dbo.Shipments", "Field21", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field22", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field23", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field24", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field25", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field26", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field27", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field28", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field29", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field30", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field31", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field32", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field33", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field34", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field35", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field36", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field37", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field38", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field39", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field40", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "ProjectNumber", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.Shipments", "ContainerLastStatusDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.Shipments", "BasicFreightId", c => c.String(maxLength: 1, unicode: false));
            AddColumn("dbo.Shipments", "DestinationPortChargesId", c => c.String(maxLength: 1, unicode: false));
            AddColumn("dbo.Shipments", "DestinationHaulageChargesId", c => c.String(maxLength: 1, unicode: false));
            AddColumn("dbo.Shipments", "AdditionalChargesId", c => c.String(maxLength: 1, unicode: false));
            AddColumn("dbo.Shipments", "FreightPayerId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Shipments", "FreightPayerAddressId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Shipments", "HasContainerException", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShipmentAdditionalCloudDatas", "DeclarationWCOXml", c => c.String());
            AddColumn("dbo.GLAccounts", "ExcludeFromDeductionReport", c => c.Boolean(nullable: false));
            AddColumn("dbo.FullAccountingSettings", "SoftwareVersion", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPackages", "IsMultiHarmonize", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShipmentPackages", "ETD", c => c.DateTime(precision: 7));
            AddColumn("dbo.ShipmentPackages", "ETA", c => c.DateTime(precision: 7));
            AddColumn("dbo.ShipmentPackages", "Routing", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.ShipmentPackages", "VoyageTripNumber", c => c.String(maxLength: 10, unicode: false));
            AddColumn("dbo.ShipmentPackages", "HasContainerException", c => c.Boolean(nullable: false));
            AddColumn("dbo.JournalLines", "ExternalReconcileNumber", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.JournalLines", "IsExternalReconcile", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteCharges", "SaleMaxAmount", c => c.Double());
            AddColumn("dbo.ShipmentPayables", "QuoteCostMinAmount", c => c.Double());
            AddColumn("dbo.ShipmentPayables", "QuoteCostMaxAmount", c => c.Double());
            AddColumn("dbo.ShipmentPickUpDeliveryPackages", "IsMultiHarmonize", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShipmentReceivables", "QuoteSaleMinAmount", c => c.Double());
            AddColumn("dbo.ShipmentReceivables", "QuoteSaleMaxAmount", c => c.Double());
            AddColumn("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode", c => c.String(maxLength: 128));
            AddColumn("dbo.TenantAdditionalDatas", "PaymentGatewayConnectionString", c => c.String());
            AddColumn("dbo.TMEmployeeTimes", "NeedsProrating", c => c.Boolean(nullable: false));
            AddColumn("dbo.TMProjects", "ExternalProjectNumber", c => c.String(maxLength: 10, unicode: false));
            AddColumn("dbo.WarehouseEntries", "ChargeableWeightUnitCode", c => c.String(maxLength: 3, unicode: false));
            AddColumn("dbo.WarehouseEntryPackages", "VolumetricWeight", c => c.Double());
            AddColumn("dbo.WarehouseReleasePackages", "VolumetricWeight", c => c.Double());
            AddColumn("dbo.WarehouseReleases", "ChargeableWeightUnitCode", c => c.String(maxLength: 3, unicode: false));
            //AlterColumn("dbo.InsideShipmentPackages", "Reference1", c => c.String(maxLength: 250, unicode: false));
            //AlterColumn("dbo.InsideShipmentPackages", "Reference2", c => c.String(maxLength: 250, unicode: false));
            //AlterColumn("dbo.InsideShipmentPackages", "Reference3", c => c.String(maxLength: 250, unicode: false));
            //AlterColumn("dbo.InsideShipmentPackages", "Reference4", c => c.String(maxLength: 250, unicode: false));
            //AlterColumn("dbo.ShipmentPackages", "Reference1", c => c.String(maxLength: 250, unicode: false));
            //AlterColumn("dbo.ShipmentPackages", "Reference2", c => c.String(maxLength: 250, unicode: false));
            //AlterColumn("dbo.ShipmentPackages", "Reference3", c => c.String(maxLength: 250, unicode: false));
            //AlterColumn("dbo.ShipmentPackages", "Reference4", c => c.String(maxLength: 250, unicode: false));
            //AlterColumn("dbo.Journals", "SearchFields", c => c.String());
            AlterColumn("dbo.TenantLoginPolicies", "SessionTimeout", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.Tenants", "LocalAddressId");
            CreateIndex("dbo.Customers", "ActivatedByUserId");
            CreateIndex("dbo.Customers", "SetAsInactiveByUserId");
            CreateIndex("dbo.Customers", "ActivationRequestedByUserId");
            CreateIndex("dbo.Tickets", "EntityType");
            CreateIndex("dbo.Queries", "SharedByUserId");
            CreateIndex("dbo.Shipments", "BasicFreightId");
            CreateIndex("dbo.Shipments", "DestinationPortChargesId");
            CreateIndex("dbo.Shipments", "DestinationHaulageChargesId");
            CreateIndex("dbo.Shipments", "AdditionalChargesId");
            CreateIndex("dbo.Shipments", "FreightPayerId");
            CreateIndex("dbo.Shipments", "FreightPayerAddressId");
            CreateIndex("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode");
            AddForeignKey("dbo.Customers", "ActivatedByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.Customers", "ActivationRequestedByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.Customers", "SetAsInactiveByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.Tenants", "LocalAddressId", "dbo.Addresses", "Id");
            AddForeignKey("dbo.Tickets", "EntityType", "dbo.ObjectTables", "Id");
            AddForeignKey("dbo.Queries", "SharedByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.Shipments", "AdditionalChargesId", "dbo.PrepaidCollects", "Id");
            AddForeignKey("dbo.Shipments", "BasicFreightId", "dbo.PrepaidCollects", "Id");
            AddForeignKey("dbo.Shipments", "DestinationHaulageChargesId", "dbo.PrepaidCollects", "Id");
            AddForeignKey("dbo.Shipments", "DestinationPortChargesId", "dbo.PrepaidCollects", "Id");
            AddForeignKey("dbo.Shipments", "FreightPayerId", "dbo.Cards", "Id");
            AddForeignKey("dbo.Shipments", "FreightPayerAddressId", "dbo.Addresses", "Id");
            AddForeignKey("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode", "dbo.PaymentGatewayPartners", "Code");
            DropColumn("dbo.GLAccounts", "IsPartOfDeductionReport");
            DropColumn("dbo.Journals", "TaxReportId");
            DropColumn("dbo.Journals", "TaxReportStatusCode");
            DropColumn("dbo.ShipmentPayables", "QuoteCostMinPrice");
            DropColumn("dbo.ShipmentReceivables", "QuoteSaleMinPrice");
            DropTable("dbo.TaxReportStatus");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TaxReportStatus",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        SearchFields = c.String(),
                        LocalName = c.String(),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            AddColumn("dbo.ShipmentReceivables", "QuoteSaleMinPrice", c => c.Double());
            AddColumn("dbo.ShipmentPayables", "QuoteCostMinPrice", c => c.Double());
            AddColumn("dbo.Journals", "TaxReportStatusCode", c => c.String(maxLength: 128));
            AddColumn("dbo.Journals", "TaxReportId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.GLAccounts", "IsPartOfDeductionReport", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode", "dbo.PaymentGatewayPartners");
            DropForeignKey("dbo.Shipments", "FreightPayerAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Shipments", "FreightPayerId", "dbo.Cards");
            DropForeignKey("dbo.Shipments", "DestinationPortChargesId", "dbo.PrepaidCollects");
            DropForeignKey("dbo.Shipments", "DestinationHaulageChargesId", "dbo.PrepaidCollects");
            DropForeignKey("dbo.Shipments", "BasicFreightId", "dbo.PrepaidCollects");
            DropForeignKey("dbo.Shipments", "AdditionalChargesId", "dbo.PrepaidCollects");
            DropForeignKey("dbo.Queries", "SharedByUserId", "dbo.Users");
            DropForeignKey("dbo.Tickets", "EntityType", "dbo.ObjectTables");
            DropForeignKey("dbo.Tenants", "LocalAddressId", "dbo.Addresses");
            DropForeignKey("dbo.CustomsShippers", "Id", "dbo.Cards");
            DropForeignKey("dbo.Customers", "SetAsInactiveByUserId", "dbo.Users");
            DropForeignKey("dbo.Customers", "ActivationRequestedByUserId", "dbo.Users");
            DropForeignKey("dbo.Customers", "ActivatedByUserId", "dbo.Users");
            DropIndex("dbo.TenantAdditionalDatas", new[] { "PaymentGatewayPartnerCode" });
            DropIndex("dbo.Shipments", new[] { "FreightPayerAddressId" });
            DropIndex("dbo.Shipments", new[] { "FreightPayerId" });
            DropIndex("dbo.Shipments", new[] { "AdditionalChargesId" });
            DropIndex("dbo.Shipments", new[] { "DestinationHaulageChargesId" });
            DropIndex("dbo.Shipments", new[] { "DestinationPortChargesId" });
            DropIndex("dbo.Shipments", new[] { "BasicFreightId" });
            DropIndex("dbo.Queries", new[] { "SharedByUserId" });
            DropIndex("dbo.Tickets", new[] { "EntityType" });
            DropIndex("dbo.CustomsShippers", new[] { "Id" });
            DropIndex("dbo.Customers", new[] { "ActivationRequestedByUserId" });
            DropIndex("dbo.Customers", new[] { "SetAsInactiveByUserId" });
            DropIndex("dbo.Customers", new[] { "ActivatedByUserId" });
            DropIndex("dbo.Tenants", new[] { "LocalAddressId" });
            AlterColumn("dbo.TenantLoginPolicies", "SessionTimeout", c => c.Int(nullable: false));
            AlterColumn("dbo.Journals", "SearchFields", c => c.String(maxLength: 1000));
            AlterColumn("dbo.ShipmentPackages", "Reference4", c => c.String(maxLength: 150, unicode: false));
            AlterColumn("dbo.ShipmentPackages", "Reference3", c => c.String(maxLength: 150, unicode: false));
            AlterColumn("dbo.ShipmentPackages", "Reference2", c => c.String(maxLength: 150, unicode: false));
            AlterColumn("dbo.ShipmentPackages", "Reference1", c => c.String(maxLength: 150, unicode: false));
            AlterColumn("dbo.InsideShipmentPackages", "Reference4", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.InsideShipmentPackages", "Reference3", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.InsideShipmentPackages", "Reference2", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.InsideShipmentPackages", "Reference1", c => c.String(maxLength: 50, unicode: false));
            DropColumn("dbo.WarehouseReleases", "ChargeableWeightUnitCode");
            DropColumn("dbo.WarehouseReleasePackages", "VolumetricWeight");
            DropColumn("dbo.WarehouseEntryPackages", "VolumetricWeight");
            DropColumn("dbo.WarehouseEntries", "ChargeableWeightUnitCode");
            DropColumn("dbo.TMProjects", "ExternalProjectNumber");
            DropColumn("dbo.TMEmployeeTimes", "NeedsProrating");
            DropColumn("dbo.TenantAdditionalDatas", "PaymentGatewayConnectionString");
            DropColumn("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode");
            DropColumn("dbo.ShipmentReceivables", "QuoteSaleMaxAmount");
            DropColumn("dbo.ShipmentReceivables", "QuoteSaleMinAmount");
            DropColumn("dbo.ShipmentPickUpDeliveryPackages", "IsMultiHarmonize");
            DropColumn("dbo.ShipmentPayables", "QuoteCostMaxAmount");
            DropColumn("dbo.ShipmentPayables", "QuoteCostMinAmount");
            DropColumn("dbo.QuoteCharges", "SaleMaxAmount");
            DropColumn("dbo.JournalLines", "IsExternalReconcile");
            DropColumn("dbo.JournalLines", "ExternalReconcileNumber");
            DropColumn("dbo.ShipmentPackages", "HasContainerException");
            DropColumn("dbo.ShipmentPackages", "VoyageTripNumber");
            DropColumn("dbo.ShipmentPackages", "Routing");
            DropColumn("dbo.ShipmentPackages", "ETA");
            DropColumn("dbo.ShipmentPackages", "ETD");
            DropColumn("dbo.ShipmentPackages", "IsMultiHarmonize");
            DropColumn("dbo.FullAccountingSettings", "SoftwareVersion");
            DropColumn("dbo.GLAccounts", "ExcludeFromDeductionReport");
            DropColumn("dbo.ShipmentAdditionalCloudDatas", "DeclarationWCOXml");
            DropColumn("dbo.Shipments", "HasContainerException");
            DropColumn("dbo.Shipments", "FreightPayerAddressId");
            DropColumn("dbo.Shipments", "FreightPayerId");
            DropColumn("dbo.Shipments", "AdditionalChargesId");
            DropColumn("dbo.Shipments", "DestinationHaulageChargesId");
            DropColumn("dbo.Shipments", "DestinationPortChargesId");
            DropColumn("dbo.Shipments", "BasicFreightId");
            DropColumn("dbo.Shipments", "ContainerLastStatusDate");
            DropColumn("dbo.Shipments", "ProjectNumber");
            DropColumn("dbo.Shipments", "Field40");
            DropColumn("dbo.Shipments", "Field39");
            DropColumn("dbo.Shipments", "Field38");
            DropColumn("dbo.Shipments", "Field37");
            DropColumn("dbo.Shipments", "Field36");
            DropColumn("dbo.Shipments", "Field35");
            DropColumn("dbo.Shipments", "Field34");
            DropColumn("dbo.Shipments", "Field33");
            DropColumn("dbo.Shipments", "Field32");
            DropColumn("dbo.Shipments", "Field31");
            DropColumn("dbo.Shipments", "Field30");
            DropColumn("dbo.Shipments", "Field29");
            DropColumn("dbo.Shipments", "Field28");
            DropColumn("dbo.Shipments", "Field27");
            DropColumn("dbo.Shipments", "Field26");
            DropColumn("dbo.Shipments", "Field25");
            DropColumn("dbo.Shipments", "Field24");
            DropColumn("dbo.Shipments", "Field23");
            DropColumn("dbo.Shipments", "Field22");
            DropColumn("dbo.Shipments", "Field21");
            DropColumn("dbo.MessagingStocks", "StockType");
            DropColumn("dbo.ARPayments", "SATApprovalDate");
            DropColumn("dbo.ARInvoices", "SATApprovalDate");
            DropColumn("dbo.APPayments", "FirstApproveDate");
            DropColumn("dbo.APInvoices", "FirstApproveDate");
            DropColumn("dbo.AgentSharedManifests", "CancelledBySenderAgent");
            DropColumn("dbo.Queries", "SpotlightModeActivated");
            DropColumn("dbo.Queries", "SharedByUserId");
            DropColumn("dbo.Queries", "SharedWithSpecificUsers");
            DropColumn("dbo.Queries", "SharedWithAll");
            DropColumn("dbo.ObjectFields", "DisplayOnLookUpLocal");
            DropColumn("dbo.Tickets", "EntityType");
            DropColumn("dbo.Customers", "ActivationRequestedByUserId");
            DropColumn("dbo.Customers", "SetAsInactiveByUserId");
            DropColumn("dbo.Customers", "ActivatedByUserId");
            DropColumn("dbo.Customers", "ActivationRequestDate");
            DropColumn("dbo.Customers", "InactiveDate");
            DropColumn("dbo.Customers", "ActivationDate");
            DropColumn("dbo.QuoteTemplateSettings", "ShowSaleMaxMinAmountContainers");
            DropColumn("dbo.QuoteTemplateSettings", "ShowSaleMaxMinAmountPackages");
            DropColumn("dbo.Users", "ShowLocalNameInLOV");
            DropColumn("dbo.Tenants", "TenantEmailSendingQuota");
            DropColumn("dbo.Tenants", "LocalAddressId");
            DropTable("dbo.PaymentGatewayPartners");
            DropTable("dbo.CustomsShippers");
            CreateIndex("dbo.Journals", "TaxReportStatusCode");
            AddForeignKey("dbo.Journals", "TaxReportStatusCode", "dbo.TaxReportStatus", "Code");
            RenameTable(name: "dbo.MessagingStockUsageHistories", newName: "AWBStockUsageHistories");
            RenameTable(name: "dbo.MessagingStocks", newName: "AWBMessagingStocks");
        }
    }
}
