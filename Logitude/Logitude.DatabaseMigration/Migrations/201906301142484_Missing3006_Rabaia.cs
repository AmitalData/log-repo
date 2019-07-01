namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Missing3006_Rabaia : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.InterfaceManagements", "InterfaceType", c => c.String(maxLength: 1, unicode: false));
            AddColumn("Customs.CustomsSettings", "CompanyType", c => c.String(nullable: false, maxLength: 1, unicode: false));
            AddColumn("Customs.ProceduralFaults", "SignedByUserId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("Customs.ProceduralFaults", "SignedByUserId");
            AddForeignKey("Customs.ProceduralFaults", "SignedByUserId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.ProceduralFaults", "SignedByUserId", "dbo.Users");
            DropIndex("Customs.ProceduralFaults", new[] { "SignedByUserId" });
            DropColumn("Customs.ProceduralFaults", "SignedByUserId");
            DropColumn("Customs.CustomsSettings", "CompanyType");
            DropColumn("Customs.InterfaceManagements", "InterfaceType");
        }
    }
}
