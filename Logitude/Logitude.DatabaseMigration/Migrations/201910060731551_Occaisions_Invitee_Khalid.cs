namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Occaisions_Invitee_Khalid : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OccasionInvitees",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        AddedDate = c.DateTime(nullable: false),
                        AddedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        SearchFields = c.String(),
                        Notes = c.String(maxLength: 4000),
                        OccasionId = c.String(maxLength: 15, unicode: false),
                        ContactId = c.String(maxLength: 15, unicode: false),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OccasionInvitees", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.OccasionInvitees", "OccasionId", "dbo.Occasions");
            DropForeignKey("dbo.OccasionInvitees", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.OccasionInvitees", "AddedByUserId", "dbo.Users");
            DropIndex("dbo.OccasionInvitees", new[] { "ContactId" });
            DropIndex("dbo.OccasionInvitees", new[] { "OccasionId" });
            DropIndex("dbo.OccasionInvitees", new[] { "UpdatedByUserId" });
            DropIndex("dbo.OccasionInvitees", new[] { "AddedByUserId" });
            DropTable("dbo.OccasionInvitees");
        }
    }
}
