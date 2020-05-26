namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CngExpDec : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.Declarations", "Direction", c => c.String(maxLength: 1, unicode: false));
            AddColumn("Customs.Declarations", "ExportFile", c => c.String(maxLength: 15, unicode: false));
            AddColumn("Customs.Declarations", "DeclarationTypeCode", c => c.String(maxLength: 3, unicode: false));
            AddColumn("Customs.Declarations", "AgentRoleCode", c => c.String(maxLength: 1, unicode: false));
            AddColumn("Customs.Declarations", "DestinationCountryCode", c => c.String(maxLength: 2, unicode: false));
            AddColumn("Customs.Declarations", "LoadingDateTime", c => c.DateTime(nullable: false, precision: 7));
            AddColumn("Customs.Declarations", "ShipCode", c => c.String(maxLength: 25, unicode: false));
            AddColumn("Customs.Declarations", "IsExporterConfirmation", c => c.Boolean(nullable: false));
            CreateIndex("Customs.Declarations", "DeclarationTypeCode");
            CreateIndex("Customs.Declarations", "DestinationCountryCode");
            CreateIndex("Customs.Declarations", "ShipCode");
            AddForeignKey("Customs.Declarations", "DestinationCountryCode", "Customs.CustomsCountries", "Code");
            AddForeignKey("Customs.Declarations", "ShipCode", "Customs.CustomsShips", "Code");
            AddForeignKey("Customs.Declarations", "DeclarationTypeCode", "Customs.LeadDocumentTypes", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.Declarations", "DeclarationTypeCode", "Customs.LeadDocumentTypes");
            DropForeignKey("Customs.Declarations", "ShipCode", "Customs.CustomsShips");
            DropForeignKey("Customs.Declarations", "DestinationCountryCode", "Customs.CustomsCountries");
            DropIndex("Customs.Declarations", new[] { "ShipCode" });
            DropIndex("Customs.Declarations", new[] { "DestinationCountryCode" });
            DropIndex("Customs.Declarations", new[] { "DeclarationTypeCode" });
            DropColumn("Customs.Declarations", "IsExporterConfirmation");
            DropColumn("Customs.Declarations", "ShipCode");
            DropColumn("Customs.Declarations", "LoadingDateTime");
            DropColumn("Customs.Declarations", "DestinationCountryCode");
            DropColumn("Customs.Declarations", "AgentRoleCode");
            DropColumn("Customs.Declarations", "DeclarationTypeCode");
            DropColumn("Customs.Declarations", "ExportFile");
            DropColumn("Customs.Declarations", "Direction");
        }
    }
}
