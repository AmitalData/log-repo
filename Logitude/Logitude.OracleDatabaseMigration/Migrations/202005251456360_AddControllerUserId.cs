namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddControllerUserId : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.DeclarationReferantDatas", "ClassifiedUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("Customs.DeclarationReferantDatas", "ControllerUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("Customs.DeclarationReferantDatas", "CollectorUserId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("Customs.DeclarationReferantDatas", "ClassifiedUserId");
            CreateIndex("Customs.DeclarationReferantDatas", "ControllerUserId");
            CreateIndex("Customs.DeclarationReferantDatas", "CollectorUserId");
            AddForeignKey("Customs.DeclarationReferantDatas", "ClassifiedUserId", "dbo.Users", "Id");
            AddForeignKey("Customs.DeclarationReferantDatas", "CollectorUserId", "dbo.Users", "Id");
            AddForeignKey("Customs.DeclarationReferantDatas", "ControllerUserId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.DeclarationReferantDatas", "ControllerUserId", "dbo.Users");
            DropForeignKey("Customs.DeclarationReferantDatas", "CollectorUserId", "dbo.Users");
            DropForeignKey("Customs.DeclarationReferantDatas", "ClassifiedUserId", "dbo.Users");
            DropIndex("Customs.DeclarationReferantDatas", new[] { "CollectorUserId" });
            DropIndex("Customs.DeclarationReferantDatas", new[] { "ControllerUserId" });
            DropIndex("Customs.DeclarationReferantDatas", new[] { "ClassifiedUserId" });
            DropColumn("Customs.DeclarationReferantDatas", "CollectorUserId");
            DropColumn("Customs.DeclarationReferantDatas", "ControllerUserId");
            DropColumn("Customs.DeclarationReferantDatas", "ClassifiedUserId");
        }
    }
}
