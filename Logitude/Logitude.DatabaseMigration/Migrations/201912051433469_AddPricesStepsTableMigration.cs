namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPricesStepsTableMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PricesSteps",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        SearchFields = c.String(),
                        Name = c.String(nullable: false, maxLength: 80),
                        Inactive = c.Boolean(nullable: false),
                        Steps = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PricesSteps", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.PricesSteps", "CreatedByUserId", "dbo.Users");
            DropIndex("dbo.PricesSteps", new[] { "UpdatedByUserId" });
            DropIndex("dbo.PricesSteps", new[] { "CreatedByUserId" });
            DropTable("dbo.PricesSteps");
        }
    }
}
