namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOtherPartnersToSharedLogisticsSettings_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SharedLogisticsSettings", "IsIssuingCarrierAgentShared", c => c.Boolean(nullable: false));
            AddColumn("dbo.SharedLogisticsSettings", "IsCustomsAgentExportShared", c => c.Boolean(nullable: false));
            AddColumn("dbo.SharedLogisticsSettings", "IsCustomsAgentImportShared", c => c.Boolean(nullable: false));
            AddColumn("dbo.SharedLogisticsSettings", "IsCustomClearancePoinShared", c => c.Boolean(nullable: false));
            AddColumn("dbo.SharedLogisticsSettings", "IsConsolidatorShared", c => c.Boolean(nullable: false));
            AddColumn("dbo.SharedLogisticsSettings", "IsReleasingAgentShared", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SharedLogisticsSettings", "IsReleasingAgentShared");
            DropColumn("dbo.SharedLogisticsSettings", "IsConsolidatorShared");
            DropColumn("dbo.SharedLogisticsSettings", "IsCustomClearancePoinShared");
            DropColumn("dbo.SharedLogisticsSettings", "IsCustomsAgentImportShared");
            DropColumn("dbo.SharedLogisticsSettings", "IsCustomsAgentExportShared");
            DropColumn("dbo.SharedLogisticsSettings", "IsIssuingCarrierAgentShared");
        }
    }
}
