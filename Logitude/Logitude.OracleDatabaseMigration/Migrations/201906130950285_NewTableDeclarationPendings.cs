namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTableDeclarationPendings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.DeclarationPendings",
                c => new
                    {
                        DeclarationID = c.String(nullable: false, maxLength: 15, unicode: false),
                        CourierPendingReasonCode = c.String(nullable: false, maxLength: 4, unicode: false),
                        Tenant = c.Int(nullable: false),
                        PendingRemarks = c.String(maxLength: 1024),
                        Status = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => new { t.DeclarationID, t.CourierPendingReasonCode })
                .ForeignKey("Customs.CourierPendingReasons", t => t.CourierPendingReasonCode)
                .ForeignKey("Customs.Declarations", t => t.DeclarationID)
                .Index(t => t.DeclarationID)
                .Index(t => t.CourierPendingReasonCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.DeclarationPendings", "DeclarationID", "Customs.Declarations");
            DropForeignKey("Customs.DeclarationPendings", "CourierPendingReasonCode", "Customs.CourierPendingReasons");
            DropIndex("Customs.DeclarationPendings", new[] { "CourierPendingReasonCode" });
            DropIndex("Customs.DeclarationPendings", new[] { "DeclarationID" });
            DropTable("Customs.DeclarationPendings");
        }
    }
}
