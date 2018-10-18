namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourierPendingReasonMigration : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "dbo.CourierPendingReasons", newSchema: "Customs");
            //DropForeignKey("Customs.DeclarationCourierStatuses", "CourierPendingReasonCode", "dbo.CourierPendingReasons");
            //DropIndex("Customs.DeclarationCourierStatuses", new[] { "CourierPendingReasonCode" });
            //DropPrimaryKey("Customs.CourierPendingReasons");
            //AlterColumn("Customs.DeclarationCourierStatuses", "CourierPendingReasonCode", c => c.String(maxLength: 4, unicode: false));
            //AlterColumn("Customs.CourierPendingReasons", "Code", c => c.String(nullable: false, maxLength: 4, unicode: false));
            //AlterColumn("Customs.CourierPendingReasons", "LocalName", c => c.String(maxLength: 100));
            //AlterColumn("Customs.CourierPendingReasons", "SearchFields", c => c.String(maxLength: 1000));
            //AlterColumn("Customs.CourierPendingReasons", "EnglishName", c => c.String(maxLength: 40, unicode: false));
            //AlterColumn("Customs.CourierPendingReasons", "ErrorPlace", c => c.String(maxLength: 1, unicode: false));
            //AlterColumn("Customs.CourierPendingReasons", "UnifreightStatusCode", c => c.String(maxLength: 3, unicode: false));
            //AddPrimaryKey("Customs.CourierPendingReasons", "Code");
            //CreateIndex("Customs.DeclarationCourierStatuses", "CourierPendingReasonCode");
            //AddForeignKey("Customs.DeclarationCourierStatuses", "CourierPendingReasonCode", "Customs.CourierPendingReasons", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.DeclarationCourierStatuses", "CourierPendingReasonCode", "Customs.CourierPendingReasons");
            DropIndex("Customs.DeclarationCourierStatuses", new[] { "CourierPendingReasonCode" });
            DropPrimaryKey("Customs.CourierPendingReasons");
            AlterColumn("Customs.CourierPendingReasons", "UnifreightStatusCode", c => c.String());
            AlterColumn("Customs.CourierPendingReasons", "ErrorPlace", c => c.String());
            AlterColumn("Customs.CourierPendingReasons", "EnglishName", c => c.String());
            AlterColumn("Customs.CourierPendingReasons", "SearchFields", c => c.String());
            AlterColumn("Customs.CourierPendingReasons", "LocalName", c => c.String());
            AlterColumn("Customs.CourierPendingReasons", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("Customs.DeclarationCourierStatuses", "CourierPendingReasonCode", c => c.String(maxLength: 128));
            AddPrimaryKey("Customs.CourierPendingReasons", "Code");
            CreateIndex("Customs.DeclarationCourierStatuses", "CourierPendingReasonCode");
            AddForeignKey("Customs.DeclarationCourierStatuses", "CourierPendingReasonCode", "dbo.CourierPendingReasons", "Code");
            MoveTable(name: "Customs.CourierPendingReasons", newSchema: "dbo");
        }
    }
}
