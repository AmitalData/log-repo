namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbCustomsAutonomyKeywords : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.CustomsAutonomyKeywords",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        KeywordtypeCode = c.String(nullable: false, maxLength: 1, unicode: false),
                        KeywordsList = c.String(maxLength: 2000),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("Customs.CustomsAutonomyKeywords");
        }
    }
}
