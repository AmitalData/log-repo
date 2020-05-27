namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class missingINTRA : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.INTTRABookingStatus", newName: "INTTRABookingStatuses");
            RenameTable(name: "dbo.INTTRABookingTransStatus", newName: "INTTRABookingTransStatuses");
            RenameTable(name: "dbo.INTTRAStatus", newName: "INTTRAStatuses");
            DropForeignKey("dbo.Shipments", "INTTRABookingStatusCode", "dbo.INTTRABookingStatus");
            DropForeignKey("dbo.Shipments", "INTTRABookingTransStatusCode", "dbo.INTTRABookingTransStatus");
            DropForeignKey("dbo.Shipments", "INTTRADocumentTypeCode", "dbo.INTTRADocumentTypes");
            DropForeignKey("dbo.Shipments", "INTTRASIStatusCode", "dbo.INTTRASIStatus");
            DropForeignKey("dbo.ShipmentPackages", "LastStatusCode", "dbo.INTTRAStatus");
            DropIndex("dbo.Shipments", new[] { "INTTRASIStatusCode" });
            DropIndex("dbo.Shipments", new[] { "INTTRADocumentTypeCode" });
            DropIndex("dbo.Shipments", new[] { "INTTRABookingTransStatusCode" });
            DropIndex("dbo.Shipments", new[] { "INTTRABookingStatusCode" });
            DropIndex("dbo.ShipmentPackages", new[] { "LastStatusCode" });
            DropPrimaryKey("dbo.INTTRABookingStatuses");
            DropPrimaryKey("dbo.INTTRABookingTransStatuses");
            DropPrimaryKey("dbo.INTTRADocumentTypes");
            DropPrimaryKey("dbo.INTTRASIStatus");
            DropPrimaryKey("dbo.INTTRAStatuses");
            CreateTable(
                "dbo.INTTRABranchRegisteredCarriers",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 15, unicode: false),
                    Tenant = c.Int(nullable: false),
                    UpdateDate = c.DateTime(precision: 7),
                    UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                    ShippingLineId = c.String(nullable: false, maxLength: 15, unicode: false),
                    BranchId = c.String(nullable: false, maxLength: 15, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .ForeignKey("dbo.ShippingLines", t => t.ShippingLineId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.ShippingLineId)
                .Index(t => t.BranchId);

            CreateTable(
                "dbo.INTTRASettingModes",
                c => new
                {
                    Code = c.String(nullable: false, maxLength: 4, unicode: false),
                    Name = c.String(nullable: false, maxLength: 20, unicode: false),
                    SearchFields = c.String(maxLength: 1000),
                })
                .PrimaryKey(t => t.Code);

            CreateTable(
                "dbo.INTTRASettings",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 15, unicode: false),
                    Tenant = c.Int(nullable: false),
                    OutSettingsId = c.String(maxLength: 15, unicode: false),
                    InSettingsId = c.String(nullable: false, maxLength: 15, unicode: false),
                    INTTRASettingModeCode = c.String(nullable: false, maxLength: 4, unicode: false),
                    INTTRAId = c.String(maxLength: 35, unicode: false),
                    INTTRAAlias = c.String(maxLength: 35, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FTPDetails", t => t.InSettingsId)
                .ForeignKey("dbo.INTTRASettingModes", t => t.INTTRASettingModeCode)
                .ForeignKey("dbo.FTPDetails", t => t.OutSettingsId)
                .Index(t => t.OutSettingsId)
                .Index(t => t.InSettingsId)
                .Index(t => t.INTTRASettingModeCode);

            AlterColumn("dbo.Shipments", "INTTRASIStatusCode", c => c.String(maxLength: 4, unicode: false));
            AlterColumn("dbo.Shipments", "INTTRADocumentTypeCode", c => c.String(maxLength: 4, unicode: false));
            AlterColumn("dbo.Shipments", "INTTRABookingTransStatusCode", c => c.String(maxLength: 4, unicode: false));
            AlterColumn("dbo.Shipments", "INTTRABookingStatusCode", c => c.String(maxLength: 4, unicode: false));
            AlterColumn("dbo.INTTRABookingStatuses", "Code", c => c.String(nullable: false, maxLength: 4, unicode: false));
            //AlterColumn("dbo.INTTRABookingStatuses", "Name", c => c.String(nullable: false, maxLength: 40, unicode: false));
            //AlterColumn("dbo.INTTRABookingStatuses", "SearchFields", c => c.String(maxLength: 1000));
            //AlterColumn("dbo.INTTRABookingTransStatuses", "Code", c => c.String(nullable: false, maxLength: 4, unicode: false));
            //AlterColumn("dbo.INTTRABookingTransStatuses", "Name", c => c.String(nullable: false, maxLength: 40, unicode: false));
            //AlterColumn("dbo.INTTRABookingTransStatuses", "SearchFields", c => c.String(maxLength: 1000));
            //AlterColumn("dbo.INTTRADocumentTypes", "Code", c => c.String(nullable: false, maxLength: 4, unicode: false));
            //AlterColumn("dbo.INTTRADocumentTypes", "Name", c => c.String(nullable: false, maxLength: 40, unicode: false));
            //AlterColumn("dbo.INTTRADocumentTypes", "SearchFields", c => c.String(maxLength: 1000));
            //AlterColumn("dbo.INTTRASIStatus", "Code", c => c.String(nullable: false, maxLength: 4, unicode: false));
            //AlterColumn("dbo.INTTRASIStatus", "Name", c => c.String(nullable: false, maxLength: 40, unicode: false));
            //AlterColumn("dbo.INTTRASIStatus", "SearchFields", c => c.String(maxLength: 1000));
            //AlterColumn("dbo.ShipmentPackages", "LastStatusCode", c => c.String(maxLength: 2, unicode: false));
            //AlterColumn("dbo.INTTRAStatuses", "Code", c => c.String(nullable: false, maxLength: 2, unicode: false));
            //AlterColumn("dbo.INTTRAStatuses", "Name", c => c.String(nullable: false, maxLength: 100, unicode: false));
            //AlterColumn("dbo.INTTRAStatuses", "SearchFields", c => c.String(maxLength: 1000));
            AddPrimaryKey("dbo.INTTRABookingStatuses", "Code");
            AddPrimaryKey("dbo.INTTRABookingTransStatuses", "Code");
            AddPrimaryKey("dbo.INTTRADocumentTypes", "Code");
            AddPrimaryKey("dbo.INTTRASIStatus", "Code");
            AddPrimaryKey("dbo.INTTRAStatuses", "Code");
            CreateIndex("dbo.Shipments", "INTTRASIStatusCode");
            CreateIndex("dbo.Shipments", "INTTRADocumentTypeCode");
            CreateIndex("dbo.Shipments", "INTTRABookingTransStatusCode");
            CreateIndex("dbo.Shipments", "INTTRABookingStatusCode");
            CreateIndex("dbo.ShipmentPackages", "LastStatusCode");
            //AddForeignKey("dbo.Shipments", "INTTRABookingStatusCode", "dbo.INTTRABookingStatuses", "Code");
            //AddForeignKey("dbo.Shipments", "INTTRABookingTransStatusCode", "dbo.INTTRABookingTransStatuses", "Code");
            //AddForeignKey("dbo.Shipments", "INTTRADocumentTypeCode", "dbo.INTTRADocumentTypes", "Code");
            //AddForeignKey("dbo.Shipments", "INTTRASIStatusCode", "dbo.INTTRASIStatus", "Code");
            //AddForeignKey("dbo.ShipmentPackages", "LastStatusCode", "dbo.INTTRAStatuses", "Code");
        }

        public override void Down()
        {
            DropForeignKey("dbo.ShipmentPackages", "LastStatusCode", "dbo.INTTRAStatuses");
            DropForeignKey("dbo.Shipments", "INTTRASIStatusCode", "dbo.INTTRASIStatus");
            DropForeignKey("dbo.Shipments", "INTTRADocumentTypeCode", "dbo.INTTRADocumentTypes");
            DropForeignKey("dbo.Shipments", "INTTRABookingTransStatusCode", "dbo.INTTRABookingTransStatuses");
            DropForeignKey("dbo.Shipments", "INTTRABookingStatusCode", "dbo.INTTRABookingStatuses");
            DropForeignKey("dbo.INTTRASettings", "OutSettingsId", "dbo.FTPDetails");
            DropForeignKey("dbo.INTTRASettings", "INTTRASettingModeCode", "dbo.INTTRASettingModes");
            DropForeignKey("dbo.INTTRASettings", "InSettingsId", "dbo.FTPDetails");
            DropForeignKey("dbo.INTTRABranchRegisteredCarriers", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.INTTRABranchRegisteredCarriers", "ShippingLineId", "dbo.ShippingLines");
            DropForeignKey("dbo.INTTRABranchRegisteredCarriers", "BranchId", "dbo.Branches");
            DropIndex("dbo.INTTRASettings", new[] { "INTTRASettingModeCode" });
            DropIndex("dbo.INTTRASettings", new[] { "InSettingsId" });
            DropIndex("dbo.INTTRASettings", new[] { "OutSettingsId" });
            DropIndex("dbo.INTTRABranchRegisteredCarriers", new[] { "BranchId" });
            DropIndex("dbo.INTTRABranchRegisteredCarriers", new[] { "ShippingLineId" });
            DropIndex("dbo.INTTRABranchRegisteredCarriers", new[] { "UpdatedByUserId" });
            DropIndex("dbo.ShipmentPackages", new[] { "LastStatusCode" });
            DropIndex("dbo.Shipments", new[] { "INTTRABookingStatusCode" });
            DropIndex("dbo.Shipments", new[] { "INTTRABookingTransStatusCode" });
            DropIndex("dbo.Shipments", new[] { "INTTRADocumentTypeCode" });
            DropIndex("dbo.Shipments", new[] { "INTTRASIStatusCode" });
            DropPrimaryKey("dbo.INTTRAStatuses");
            DropPrimaryKey("dbo.INTTRASIStatus");
            DropPrimaryKey("dbo.INTTRADocumentTypes");
            DropPrimaryKey("dbo.INTTRABookingTransStatuses");
            DropPrimaryKey("dbo.INTTRABookingStatuses");
            AlterColumn("dbo.INTTRAStatuses", "SearchFields", c => c.String());
            AlterColumn("dbo.INTTRAStatuses", "Name", c => c.String());
            AlterColumn("dbo.INTTRAStatuses", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ShipmentPackages", "LastStatusCode", c => c.String(maxLength: 128));
            AlterColumn("dbo.INTTRASIStatus", "SearchFields", c => c.String());
            AlterColumn("dbo.INTTRASIStatus", "Name", c => c.String());
            AlterColumn("dbo.INTTRASIStatus", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.INTTRADocumentTypes", "SearchFields", c => c.String());
            AlterColumn("dbo.INTTRADocumentTypes", "Name", c => c.String());
            AlterColumn("dbo.INTTRADocumentTypes", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.INTTRABookingTransStatuses", "SearchFields", c => c.String());
            AlterColumn("dbo.INTTRABookingTransStatuses", "Name", c => c.String());
            AlterColumn("dbo.INTTRABookingTransStatuses", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.INTTRABookingStatuses", "SearchFields", c => c.String());
            AlterColumn("dbo.INTTRABookingStatuses", "Name", c => c.String());
            AlterColumn("dbo.INTTRABookingStatuses", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Shipments", "INTTRABookingStatusCode", c => c.String(maxLength: 128));
            AlterColumn("dbo.Shipments", "INTTRABookingTransStatusCode", c => c.String(maxLength: 128));
            AlterColumn("dbo.Shipments", "INTTRADocumentTypeCode", c => c.String(maxLength: 128));
            AlterColumn("dbo.Shipments", "INTTRASIStatusCode", c => c.String(maxLength: 128));
            DropTable("dbo.INTTRASettings");
            DropTable("dbo.INTTRASettingModes");
            DropTable("dbo.INTTRABranchRegisteredCarriers");
            AddPrimaryKey("dbo.INTTRAStatuses", "Code");
            AddPrimaryKey("dbo.INTTRASIStatus", "Code");
            AddPrimaryKey("dbo.INTTRADocumentTypes", "Code");
            AddPrimaryKey("dbo.INTTRABookingTransStatuses", "Code");
            AddPrimaryKey("dbo.INTTRABookingStatuses", "Code");
            CreateIndex("dbo.ShipmentPackages", "LastStatusCode");
            CreateIndex("dbo.Shipments", "INTTRABookingStatusCode");
            CreateIndex("dbo.Shipments", "INTTRABookingTransStatusCode");
            CreateIndex("dbo.Shipments", "INTTRADocumentTypeCode");
            CreateIndex("dbo.Shipments", "INTTRASIStatusCode");
            AddForeignKey("dbo.ShipmentPackages", "LastStatusCode", "dbo.INTTRAStatus", "Code");
            AddForeignKey("dbo.Shipments", "INTTRASIStatusCode", "dbo.INTTRASIStatus", "Code");
            AddForeignKey("dbo.Shipments", "INTTRADocumentTypeCode", "dbo.INTTRADocumentTypes", "Code");
            AddForeignKey("dbo.Shipments", "INTTRABookingTransStatusCode", "dbo.INTTRABookingTransStatus", "Code");
            AddForeignKey("dbo.Shipments", "INTTRABookingStatusCode", "dbo.INTTRABookingStatus", "Code");
            RenameTable(name: "dbo.INTTRAStatuses", newName: "INTTRAStatus");
            RenameTable(name: "dbo.INTTRABookingTransStatuses", newName: "INTTRABookingTransStatus");
            RenameTable(name: "dbo.INTTRABookingStatuses", newName: "INTTRABookingStatus");
        }
    }
}
