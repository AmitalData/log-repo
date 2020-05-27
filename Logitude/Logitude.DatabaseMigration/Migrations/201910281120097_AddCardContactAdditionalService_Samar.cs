namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCardContactAdditionalService_Samar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CardContactAdditionalServices",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CardContactId = c.String(nullable: false, maxLength: 15, unicode: false),
                        AdditionalServiceId = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AdditionalServices", t => t.AdditionalServiceId)
                .ForeignKey("dbo.CardContacts", t => t.CardContactId)
                .Index(t => t.CardContactId)
                .Index(t => t.AdditionalServiceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CardContactAdditionalServices", "CardContactId", "dbo.CardContacts");
            DropForeignKey("dbo.CardContactAdditionalServices", "AdditionalServiceId", "dbo.AdditionalServices");            
            DropIndex("dbo.CardContactAdditionalServices", new[] { "AdditionalServiceId" });
            DropIndex("dbo.CardContactAdditionalServices", new[] { "CardContactId" });            
            DropTable("dbo.CardContactAdditionalServices");            
        }
    }
}
