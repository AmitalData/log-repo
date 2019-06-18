namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigrationDelMamanStatusCodeDec : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Customs.Declarations", "MamanStatusCode", "Customs.MamanStatuses");
            DropIndex("Customs.Declarations", new[] { "MamanStatusCode" });
            DropColumn("Customs.Declarations", "MamanStatusCode");
            DropColumn("Customs.Declarations", "MamanErrorXml");
        }
        
        public override void Down()
        {
            AddColumn("Customs.Declarations", "MamanErrorXml", c => c.String(unicode: false));
            AddColumn("Customs.Declarations", "MamanStatusCode", c => c.String(maxLength: 3, unicode: false));
            CreateIndex("Customs.Declarations", "MamanStatusCode");
            AddForeignKey("Customs.Declarations", "MamanStatusCode", "Customs.MamanStatuses", "Code");
        }
    }
}
