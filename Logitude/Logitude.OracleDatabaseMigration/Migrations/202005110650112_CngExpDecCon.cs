namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CngExpDecCon : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.Consignments", "IsDangerousGoods", c => c.Boolean(nullable: false));
            AddColumn("Customs.Consignments", "FinalDestinationPortCode", c => c.String(maxLength: 17, unicode: false));
            CreateIndex("Customs.Consignments", "FinalDestinationPortCode");
            AddForeignKey("Customs.Consignments", "FinalDestinationPortCode", "Customs.InternationalSites", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.Consignments", "FinalDestinationPortCode", "Customs.InternationalSites");
            DropIndex("Customs.Consignments", new[] { "FinalDestinationPortCode" });
            DropColumn("Customs.Consignments", "FinalDestinationPortCode");
            DropColumn("Customs.Consignments", "IsDangerousGoods");
        }
    }
}
