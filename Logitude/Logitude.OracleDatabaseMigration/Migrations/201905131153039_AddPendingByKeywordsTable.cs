namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPendingByKeywordsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.PendingByKeywords",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CourierPendingReasonCode = c.String(maxLength: 4, unicode: false),
                        KeywordsList = c.String(maxLength: 2000),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Customs.CourierPendingReasons", t => t.CourierPendingReasonCode)
                .Index(t => t.CourierPendingReasonCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.PendingByKeywords", "CourierPendingReasonCode", "Customs.CourierPendingReasons");
            DropIndex("Customs.PendingByKeywords", new[] { "CourierPendingReasonCode" });
            DropTable("Customs.PendingByKeywords");
        }
    }
}
