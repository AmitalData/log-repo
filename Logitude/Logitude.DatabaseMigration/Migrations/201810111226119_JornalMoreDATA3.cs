namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JornalMoreDATA3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JournalMoreDatas",
                c => new
                    {
                        JournalId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Line = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        GeneralData = c.String(nullable: false, maxLength: 4000),
                    })
                .PrimaryKey(t => new { t.JournalId, t.Line })
                .ForeignKey("dbo.Journals", t => t.JournalId)
                .Index(t => t.JournalId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JournalMoreDatas", "JournalId", "dbo.Journals");
            DropIndex("dbo.JournalMoreDatas", new[] { "JournalId" });
            DropTable("dbo.JournalMoreDatas");
        }
    }
}
