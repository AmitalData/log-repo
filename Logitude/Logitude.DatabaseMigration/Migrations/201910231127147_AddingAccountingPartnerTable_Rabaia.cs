namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingAccountingPartnerTable_Rabaia : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountingPartners",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        PrimaryContactName = c.String(maxLength: 60, unicode: false),
                        PrimaryContactEmail = c.String(maxLength: 70, unicode: false),
                        PrimaryContactPhone = c.String(maxLength: 25, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cards", t => t.Id)
                .Index(t => t.Id);
            
            //AlterColumn("dbo.JournalLines", "Notes", c => c.String(maxLength: 250));
            //DropColumn("dbo.CustomsTransferLines", "SearchFields");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomsTransferLines", "SearchFields", c => c.String(maxLength: 1000));
            DropForeignKey("dbo.AccountingPartners", "Id", "dbo.Cards");
            DropIndex("dbo.AccountingPartners", new[] { "Id" });
            AlterColumn("dbo.JournalLines", "Notes", c => c.String(maxLength: 60));
            DropTable("dbo.AccountingPartners");
        }
    }
}
