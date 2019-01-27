namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddToggleTablesToDB_Samar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Toggles",
                c => new
                {
                    Code = c.String(nullable: false, maxLength: 3, unicode: false),
                    Name = c.String(nullable: false, maxLength: 3, unicode: false),
                    SearchFields = c.String(maxLength: 1000),
                })
                .PrimaryKey(t => t.Code);

            CreateTable(
                "dbo.FeatureToggles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        SearchFields = c.String(maxLength: 1000),
                        TenantNumber = c.Int(nullable: false),
                        Inactive = c.Boolean(nullable: false),
                        ToggleCode = c.String(nullable: false, maxLength: 3, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Toggles", t => t.ToggleCode)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.ToggleCode); 
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FeatureToggles", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.FeatureToggles", "ToggleCode", "dbo.Toggles");
            DropForeignKey("dbo.FeatureToggles", "CreatedByUserId", "dbo.Users");
            DropIndex("dbo.FeatureToggles", new[] { "ToggleCode" });
            DropIndex("dbo.FeatureToggles", new[] { "UpdatedByUserId" });
            DropIndex("dbo.FeatureToggles", new[] { "CreatedByUserId" });
            DropTable("dbo.Toggles");
            DropTable("dbo.FeatureToggles");
        }
    }
}
