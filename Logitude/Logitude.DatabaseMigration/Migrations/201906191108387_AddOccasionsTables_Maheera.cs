namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOccasionsTables_Maheera : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Occasions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        SearchFields = c.String(),
                        Name = c.String(nullable: false, maxLength: 100),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        Goal = c.String(maxLength: 500),
                        Location = c.String(maxLength: 500),
                        OwnerId = c.String(maxLength: 15, unicode: false),
                        IndustryId = c.String(maxLength: 15, unicode: false),
                        OccasionTypeId = c.String(maxLength: 15, unicode: false),
                        OccasionStatusId = c.String(maxLength: 3, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Industries", t => t.IndustryId)
                .ForeignKey("dbo.OccasionStatuses", t => t.OccasionStatusId)
                .ForeignKey("dbo.OccasionTypes", t => t.OccasionTypeId)
                .ForeignKey("dbo.Users", t => t.OwnerId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.OwnerId)
                .Index(t => t.IndustryId)
                .Index(t => t.OccasionTypeId)
                .Index(t => t.OccasionStatusId);
            
            CreateTable(
                "dbo.OccasionStatuses",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 3, unicode: false),
                        Name = c.String(maxLength: 60, unicode: false),
                        SearchFields = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.OccasionTypes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        SearchFields = c.String(),
                        Code = c.String(maxLength: 3, unicode: false),
                        Name = c.String(maxLength: 100),
                        AddedManually = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Occasions", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Occasions", "OwnerId", "dbo.Users");
            DropForeignKey("dbo.Occasions", "OccasionTypeId", "dbo.OccasionTypes");
            DropForeignKey("dbo.OccasionTypes", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.OccasionTypes", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Occasions", "OccasionStatusId", "dbo.OccasionStatuses");
            DropForeignKey("dbo.Occasions", "IndustryId", "dbo.Industries");
            DropForeignKey("dbo.Occasions", "CreatedByUserId", "dbo.Users");
            DropIndex("dbo.OccasionTypes", new[] { "UpdatedByUserId" });
            DropIndex("dbo.OccasionTypes", new[] { "CreatedByUserId" });
            DropIndex("dbo.Occasions", new[] { "OccasionStatusId" });
            DropIndex("dbo.Occasions", new[] { "OccasionTypeId" });
            DropIndex("dbo.Occasions", new[] { "IndustryId" });
            DropIndex("dbo.Occasions", new[] { "OwnerId" });
            DropIndex("dbo.Occasions", new[] { "UpdatedByUserId" });
            DropIndex("dbo.Occasions", new[] { "CreatedByUserId" });
            DropTable("dbo.OccasionTypes");
            DropTable("dbo.OccasionStatuses");
            DropTable("dbo.Occasions");
        }
    }
}
