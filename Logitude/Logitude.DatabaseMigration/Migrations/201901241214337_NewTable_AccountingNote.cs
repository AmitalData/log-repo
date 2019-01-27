namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTable_AccountingNote : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountingNotes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        CardId = c.String(maxLength: 15, unicode: false),
                        Notes = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cards", t => t.CardId)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.CardId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccountingNotes", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.AccountingNotes", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.AccountingNotes", "CardId", "dbo.Cards");
            DropIndex("dbo.AccountingNotes", new[] { "CardId" });
            DropIndex("dbo.AccountingNotes", new[] { "UpdatedByUserId" });
            DropIndex("dbo.AccountingNotes", new[] { "CreatedByUserId" });
            DropTable("dbo.AccountingNotes");
        }
    }
}
