namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Abed_AddCourierCustomStatusToLogitudeContext_PocoAndMap : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.CourierCustomStatus", newName: "CourierCustomStatuses");
            //MoveTable(name: "dbo.CourierCustomStatuses", newSchema: "Customs");
            DropForeignKey("Customs.Declarations", "FK_Customs.Declarations_Customs.CourierCustomStatus_CourierCustomStatusCode");
            DropIndex("Customs.Declarations", "IX_CourierCustomStatusCode");
            DropPrimaryKey("Customs.CourierCustomStatuses", "PK_Customs.CourierCustomStatus");
            //AddColumn("dbo.Users", "LayoutDirection", c => c.String(maxLength: 3, unicode: false));
            //AddColumn("dbo.TariffSettings", "ContainerDefaults", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("Customs.Declarations", "CourierCustomStatusCode", c => c.String(maxLength: 3, unicode: false));
            AlterColumn("Customs.CourierCustomStatuses", "Code", c => c.String(nullable: false, maxLength: 3, unicode: false));
            AlterColumn("Customs.CourierCustomStatuses", "Name", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("Customs.CourierCustomStatuses", "SearchFields", c => c.String(maxLength: 1000));
            AlterColumn("Customs.CourierCustomStatuses", "LocalName", c => c.String(maxLength: 100));
            AddPrimaryKey("Customs.CourierCustomStatuses", "Code");
            CreateIndex("Customs.Declarations", "CourierCustomStatusCode");
            AddForeignKey("Customs.Declarations", "CourierCustomStatusCode", "Customs.CourierCustomStatuses", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.Declarations", "FK_Customs.Declarations_Customs.CourierCustomStatus_CourierCustomStatusCode");
            DropIndex("Customs.Declarations", "IX_CourierCustomStatusCode");
            DropPrimaryKey("Customs.CourierCustomStatuses", "PK_Customs.CourierCustomStatus");
            AlterColumn("Customs.CourierCustomStatuses", "LocalName", c => c.String());
            AlterColumn("Customs.CourierCustomStatuses", "SearchFields", c => c.String());
            AlterColumn("Customs.CourierCustomStatuses", "Name", c => c.String());
            AlterColumn("Customs.CourierCustomStatuses", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("Customs.Declarations", "CourierCustomStatusCode", c => c.String(maxLength: 128));
            //DropColumn("dbo.TariffSettings", "ContainerDefaults");
            //DropColumn("dbo.Users", "LayoutDirection");
            AddPrimaryKey("Customs.CourierCustomStatuses", "Code");
            CreateIndex("Customs.Declarations", "CourierCustomStatusCode");
            AddForeignKey("Customs.Declarations", "CourierCustomStatusCode", "dbo.CourierCustomStatus", "Code");
            //MoveTable(name: "Customs.CourierCustomStatuses", newSchema: "dbo");
            //RenameTable(name: "dbo.CourierCustomStatuses", newName: "CourierCustomStatus");
        }
    }
}
