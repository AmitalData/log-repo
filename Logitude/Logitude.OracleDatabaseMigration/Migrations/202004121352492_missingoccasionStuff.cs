namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missingoccasionStuff : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OccasionInvitees",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        AddedDate = c.DateTime(nullable: false, precision: 7),
                        AddedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false, precision: 7),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        SearchFields = c.String(),
                        Notes = c.String(maxLength: 2000),
                        OccasionId = c.String(maxLength: 15, unicode: false),
                        ContactId = c.String(maxLength: 15, unicode: false),
                        Invited = c.Boolean(nullable: false),
                        Participated = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AddedByUserId)
                .ForeignKey("dbo.Contacts", t => t.ContactId)
                .ForeignKey("dbo.Occasions", t => t.OccasionId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.AddedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.OccasionId)
                .Index(t => t.ContactId);
            
            CreateTable(
                "dbo.Occasions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false, precision: 7),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false, precision: 7),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        SearchFields = c.String(),
                        Name = c.String(nullable: false, maxLength: 100),
                        StartDateTime = c.DateTime(precision: 7),
                        EndDateTime = c.DateTime(precision: 7),
                        Goal = c.String(maxLength: 500),
                        Location = c.String(maxLength: 500),
                        OwnerId = c.String(maxLength: 15, unicode: false),
                        IndustryId = c.String(maxLength: 15, unicode: false),
                        OccasionTypeId = c.String(maxLength: 15, unicode: false),
                        OccasionStatusId = c.String(maxLength: 3, unicode: false),
                        ParticipatedCustomers = c.Int(nullable: false),
                        ParticipatedContacts = c.Int(nullable: false),
                        InvitedCustomers = c.Int(nullable: false),
                        InvitedContacts = c.Int(nullable: false),
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
                        CreateDate = c.DateTime(nullable: false, precision: 7),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false, precision: 7),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        SearchFields = c.String(),
                        Code = c.String(nullable: false, maxLength: 3, unicode: false),
                        Name = c.String(nullable: false, maxLength: 100),
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
            DropForeignKey("dbo.OccasionInvitees", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.OccasionInvitees", "OccasionId", "dbo.Occasions");
            DropForeignKey("dbo.Occasions", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Occasions", "OwnerId", "dbo.Users");
            DropForeignKey("dbo.Occasions", "OccasionTypeId", "dbo.OccasionTypes");
            DropForeignKey("dbo.OccasionTypes", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.OccasionTypes", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Occasions", "OccasionStatusId", "dbo.OccasionStatuses");
            DropForeignKey("dbo.Occasions", "IndustryId", "dbo.Industries");
            DropForeignKey("dbo.Occasions", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.OccasionInvitees", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.OccasionInvitees", "AddedByUserId", "dbo.Users");
            DropIndex("dbo.OccasionTypes", new[] { "UpdatedByUserId" });
            DropIndex("dbo.OccasionTypes", new[] { "CreatedByUserId" });
            DropIndex("dbo.Occasions", new[] { "OccasionStatusId" });
            DropIndex("dbo.Occasions", new[] { "OccasionTypeId" });
            DropIndex("dbo.Occasions", new[] { "IndustryId" });
            DropIndex("dbo.Occasions", new[] { "OwnerId" });
            DropIndex("dbo.Occasions", new[] { "UpdatedByUserId" });
            DropIndex("dbo.Occasions", new[] { "CreatedByUserId" });
            DropIndex("dbo.OccasionInvitees", new[] { "ContactId" });
            DropIndex("dbo.OccasionInvitees", new[] { "OccasionId" });
            DropIndex("dbo.OccasionInvitees", new[] { "UpdatedByUserId" });
            DropIndex("dbo.OccasionInvitees", new[] { "AddedByUserId" });
            DropTable("dbo.OccasionTypes");
            DropTable("dbo.OccasionStatuses");
            DropTable("dbo.Occasions");
            DropTable("dbo.OccasionInvitees");
        }
    }
}
