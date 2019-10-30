namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SharedLogisticsContactLastLogin_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SharedLogisticsContactLastLogins",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        LoginDateTime = c.DateTime(),
                        Tenant = c.Int(nullable: false),
                        CardId = c.String(maxLength: 15, unicode: false),
                        PartnerTypeId = c.String(maxLength: 2, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SharedLogisticsContactLastLogins", "Id", "dbo.Contacts");
            DropIndex("dbo.SharedLogisticsContactLastLogins", new[] { "Id" });
            DropTable("dbo.SharedLogisticsContactLastLogins");
        }
    }
}
