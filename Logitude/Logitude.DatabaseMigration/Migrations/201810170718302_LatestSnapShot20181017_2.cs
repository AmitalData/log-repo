namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LatestSnapShot20181017_2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Customs.Declarations", "AcceptanceStatusCode", "dbo.AcceptanceStatus");
            DropIndex("Customs.Declarations", new[] { "AcceptanceStatusCode" });
            DropPrimaryKey("dbo.AcceptanceStatus");
            RenameTable(name: "dbo.AcceptanceStatus", newName: "AcceptanceStatuses");
            MoveTable(name: "dbo.AcceptanceStatuses", newSchema: "Customs");
            
            
            AlterColumn("Customs.Declarations", "AcceptanceStatusCode", c => c.String(maxLength: 3, unicode: false));
            AlterColumn("Customs.AcceptanceStatuses", "Code", c => c.String(nullable: false, maxLength: 3, unicode: false));
            AlterColumn("Customs.AcceptanceStatuses", "Name", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("Customs.AcceptanceStatuses", "SearchFields", c => c.String(maxLength: 1000));
            AlterColumn("Customs.AcceptanceStatuses", "LocalName", c => c.String(maxLength: 100));
            AddPrimaryKey("Customs.AcceptanceStatuses", "Code");
            CreateIndex("Customs.Declarations", "AcceptanceStatusCode");
            AddForeignKey("Customs.Declarations", "AcceptanceStatusCode", "Customs.AcceptanceStatuses", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.Declarations", "AcceptanceStatusCode", "Customs.AcceptanceStatuses");
            DropIndex("Customs.Declarations", new[] { "AcceptanceStatusCode" });
            DropPrimaryKey("Customs.AcceptanceStatuses");
            AlterColumn("Customs.AcceptanceStatuses", "LocalName", c => c.String());
            AlterColumn("Customs.AcceptanceStatuses", "SearchFields", c => c.String());
            AlterColumn("Customs.AcceptanceStatuses", "Name", c => c.String());
            AlterColumn("Customs.AcceptanceStatuses", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("Customs.Declarations", "AcceptanceStatusCode", c => c.String(maxLength: 128));
            AddPrimaryKey("Customs.AcceptanceStatuses", "Code");
            CreateIndex("Customs.Declarations", "AcceptanceStatusCode");
            AddForeignKey("Customs.Declarations", "AcceptanceStatusCode", "dbo.AcceptanceStatus", "Code");
            MoveTable(name: "Customs.AcceptanceStatuses", newSchema: "dbo");
            RenameTable(name: "dbo.AcceptanceStatuses", newName: "AcceptanceStatus");
        }
    }
}
