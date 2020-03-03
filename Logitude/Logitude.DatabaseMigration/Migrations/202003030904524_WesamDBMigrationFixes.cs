namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamDBMigrationFixes : DbMigration
    {
        public override void Up()
        {
            //DropIndex("dbo.Tenants", new[] { "PasswordPolicyCode" });
            //DropIndex("dbo.QueueMessageMoreDetails", new[] { "QueueDefinitionCode" });
            //AddColumn("dbo.Cards", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Users", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Branches", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Addresses", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Countries", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.States", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Contacts", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Departments", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Customers", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Ranks", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Currencies", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Tenants", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.PartnerTypes", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Directions", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Ports", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Incoterms", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.TransportModes", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.ShipmentTypes", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.ShipmentLevels", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Shipments", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.EntityStatus", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.ShipmentMasterDatas", "AutomaticLastUpdateDate", c => c.DateTime());
            //AlterColumn("dbo.Contacts", "ExternalId", c => c.String(maxLength: 20, unicode: false));
            //AlterColumn("dbo.Tenants", "PasswordPolicyCode", c => c.String(maxLength: 4, unicode: false));
            //AlterColumn("dbo.ObjectTables", "CodeField", c => c.String(maxLength: 60, unicode: false));
            //AlterColumn("dbo.ObjectTables", "SplitComponentPath", c => c.String());
            //AlterColumn("dbo.Features", "Code", c => c.String(nullable: false, maxLength: 120, unicode: false));
            //AlterColumn("dbo.ShipmentAdditionalCloudDatas", "ApprovedByUserName", c => c.String(maxLength: 200));
            //AlterColumn("dbo.ShipmentAdditionalCloudDatas", "VersionApproved", c => c.String(maxLength: 10));
            //AlterColumn("dbo.ShipmentAdditionalCloudDatas", "DenyReason", c => c.String(maxLength: 1024));
            //AlterColumn("dbo.ShipmentAdditionalCloudDatas", "ShipmentAddtionalDataXML", c => c.String(maxLength: 4000));
            //AlterColumn("dbo.ShipmentAdditionalCloudDatas", "UserIdNumber", c => c.String(maxLength: 35));
            //AlterColumn("dbo.Followers", "CancelledDate", c => c.DateTime());
            //AlterColumn("dbo.QueueMessageMoreDetails", "QueueDefinitionCode", c => c.String(nullable: false, maxLength: 265, unicode: false));
            //AlterColumn("dbo.QueueMessageMoreDetails", "MessageBody", c => c.String(nullable: false, maxLength: 1000, unicode: false));
            //AlterColumn("dbo.QueueMessageMoreDetails", "Field1", c => c.String(maxLength: 15, unicode: false));
            //AlterColumn("dbo.QueueMessageMoreDetails", "Field2", c => c.String(maxLength: 15, unicode: false));
            //AlterColumn("dbo.QueueMessageMoreDetails", "Field3", c => c.String(maxLength: 15, unicode: false));
            //CreateIndex("dbo.Tenants", "PasswordPolicyCode");
            //CreateIndex("dbo.QueueMessageMoreDetails", "QueueDefinitionCode");
        }

        public override void Down()
        {
            //DropIndex("dbo.QueueMessageMoreDetails", new[] { "QueueDefinitionCode" });
            //DropIndex("dbo.Tenants", new[] { "PasswordPolicyCode" });
            //AlterColumn("dbo.QueueMessageMoreDetails", "Field3", c => c.String());
            //AlterColumn("dbo.QueueMessageMoreDetails", "Field2", c => c.String());
            //AlterColumn("dbo.QueueMessageMoreDetails", "Field1", c => c.String());
            //AlterColumn("dbo.QueueMessageMoreDetails", "MessageBody", c => c.String());
            //AlterColumn("dbo.QueueMessageMoreDetails", "QueueDefinitionCode", c => c.String(maxLength: 265, unicode: false));
            //AlterColumn("dbo.Followers", "CancelledDate", c => c.Boolean());
            //AlterColumn("dbo.ShipmentAdditionalCloudDatas", "UserIdNumber", c => c.String());
            //AlterColumn("dbo.ShipmentAdditionalCloudDatas", "ShipmentAddtionalDataXML", c => c.String());
            //AlterColumn("dbo.ShipmentAdditionalCloudDatas", "DenyReason", c => c.String());
            //AlterColumn("dbo.ShipmentAdditionalCloudDatas", "VersionApproved", c => c.String());
            //AlterColumn("dbo.ShipmentAdditionalCloudDatas", "ApprovedByUserName", c => c.String());
            //AlterColumn("dbo.Features", "Code", c => c.String(nullable: false, maxLength: 80, unicode: false));
            //AlterColumn("dbo.ObjectTables", "SplitComponentPath", c => c.String(maxLength: 250));
            //AlterColumn("dbo.ObjectTables", "CodeField", c => c.String(maxLength: 15, unicode: false));
            //AlterColumn("dbo.Tenants", "PasswordPolicyCode", c => c.String(nullable: false, maxLength: 4, unicode: false));
            //AlterColumn("dbo.Contacts", "ExternalId", c => c.String());
            //DropColumn("dbo.ShipmentMasterDatas", "AutomaticLastUpdateDate");
            //DropColumn("dbo.EntityStatus", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Shipments", "AutomaticLastUpdateDate");
            //DropColumn("dbo.ShipmentLevels", "AutomaticLastUpdateDate");
            //DropColumn("dbo.ShipmentTypes", "AutomaticLastUpdateDate");
            //DropColumn("dbo.TransportModes", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Incoterms", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Ports", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Directions", "AutomaticLastUpdateDate");
            //DropColumn("dbo.PartnerTypes", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Tenants", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Currencies", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Ranks", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Customers", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Departments", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Contacts", "AutomaticLastUpdateDate");
            //DropColumn("dbo.States", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Countries", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Addresses", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Branches", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Users", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Cards", "AutomaticLastUpdateDate");
            //CreateIndex("dbo.QueueMessageMoreDetails", "QueueDefinitionCode");
            //CreateIndex("dbo.Tenants", "PasswordPolicyCode");
        }
    }
}
