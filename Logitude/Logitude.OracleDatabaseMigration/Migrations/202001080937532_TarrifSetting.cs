namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TarrifSetting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TariffSettings",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        DefaultPriceSteps = c.String(nullable: false, maxLength: 100, unicode: false),
                        Tenant = c.Int(nullable: false),
                        DefaultWarningPercentage = c.Double(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TariffSettings");
        }
    }
}
