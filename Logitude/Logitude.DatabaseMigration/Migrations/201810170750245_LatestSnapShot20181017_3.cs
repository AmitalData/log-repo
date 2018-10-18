namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LatestSnapShot20181017_3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Customs.Declarations", "MamanStatusCode", "dbo.MamanStatus");
            DropIndex("Customs.Declarations", new[] { "MamanStatusCode" });
            DropPrimaryKey("dbo.MamanStatus");
            RenameTable(name: "dbo.MamanStatus", newName: "MamanStatuses");
            MoveTable(name: "dbo.MamanStatuses", newSchema: "Customs");
            
            AlterColumn("Customs.Declarations", "MamanStatusCode", c => c.String(maxLength: 3, unicode: false));
            AlterColumn("Customs.MamanStatuses", "Code", c => c.String(nullable: false, maxLength: 3, unicode: false));
            AlterColumn("Customs.MamanStatuses", "Name", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("Customs.MamanStatuses", "SearchFields", c => c.String(maxLength: 1000));
            AlterColumn("Customs.MamanStatuses", "LocalName", c => c.String(maxLength: 100));
            AddPrimaryKey("Customs.MamanStatuses", "Code");
            CreateIndex("Customs.Declarations", "MamanStatusCode");
            AddForeignKey("Customs.Declarations", "MamanStatusCode", "Customs.MamanStatuses", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.Declarations", "MamanStatusCode", "Customs.MamanStatuses");
            DropIndex("Customs.Declarations", new[] { "MamanStatusCode" });
            DropPrimaryKey("Customs.MamanStatuses");
            AlterColumn("Customs.MamanStatuses", "LocalName", c => c.String());
            AlterColumn("Customs.MamanStatuses", "SearchFields", c => c.String());
            AlterColumn("Customs.MamanStatuses", "Name", c => c.String());
            AlterColumn("Customs.MamanStatuses", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("Customs.Declarations", "MamanStatusCode", c => c.String(maxLength: 128));
            AddPrimaryKey("Customs.MamanStatuses", "Code");
            CreateIndex("Customs.Declarations", "MamanStatusCode");
            AddForeignKey("Customs.Declarations", "MamanStatusCode", "dbo.MamanStatus", "Code");
            MoveTable(name: "Customs.MamanStatuses", newSchema: "dbo");
            RenameTable(name: "dbo.MamanStatuses", newName: "MamanStatus");
        }
    }
}
