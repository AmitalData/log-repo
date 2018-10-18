namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSharedLogisticsSettingsTable : DbMigration
    {
        public override void Up()
        {           
            CreateTable(
                "dbo.SharedLogisticsSettings",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
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
                    })
                .PrimaryKey(t => t.Id);            
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SharedLogisticsSettings");            
        }
    }
}
