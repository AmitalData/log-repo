namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inframissing : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SharedLogisticsSettings",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Tenant = c.Int(nullable: false),
                        IsAgentShared = c.Boolean(nullable: false),
                        IsShipperNotExporterShared = c.Boolean(nullable: false),
                        IsNotify1Shared = c.Boolean(nullable: false),
                        IsNotify2Shared = c.Boolean(nullable: false),
                        IsFreightForwarderShared = c.Boolean(nullable: false),
                        IsColoaderShared = c.Boolean(nullable: false),
                        IsConsigneeNotImporterShared = c.Boolean(nullable: false),
                        IsMainCarrierShared = c.Boolean(nullable: false),
                        IsPickDelivCarriesShared = c.Boolean(nullable: false),
                        IsInvoicesMenuEnabled = c.Boolean(nullable: false),
                        IsMoneyTabEnabled = c.Boolean(nullable: false),
                        IsIssuingCarrierAgentShared = c.Boolean(nullable: false),
                        IsCustomsAgentExportShared = c.Boolean(nullable: false),
                        IsCustomsAgentImportShared = c.Boolean(nullable: false),
                        IsCustomClearancePoinShared = c.Boolean(nullable: false),
                        IsConsolidatorShared = c.Boolean(nullable: false),
                        IsReleasingAgentShared = c.Boolean(nullable: false),
                        IsShipperShared = c.Boolean(nullable: false),
                        IsConsigneeShared = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SharedLogisticsSettings");
        }
    }
}
