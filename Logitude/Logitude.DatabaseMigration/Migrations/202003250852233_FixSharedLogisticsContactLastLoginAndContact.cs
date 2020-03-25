namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSharedLogisticsContactLastLoginAndContact : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Contacts", new[] { "SharedLogisticsContactLastLogin_ContactId", "SharedLogisticsContactLastLogin_CardId", "SharedLogisticsContactLastLogin_PartnerTypeId", "SharedLogisticsContactLastLogin_Via" }, "dbo.SharedLogisticsContactLastLogins");
            DropIndex("dbo.Contacts", new[] { "SharedLogisticsContactLastLogin_ContactId", "SharedLogisticsContactLastLogin_CardId", "SharedLogisticsContactLastLogin_PartnerTypeId", "SharedLogisticsContactLastLogin_Via" });
            CreateIndex("dbo.SharedLogisticsContactLastLogins", "ContactId");
            AddForeignKey("dbo.SharedLogisticsContactLastLogins", "ContactId", "dbo.Contacts", "Id");
            DropColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_ContactId");
            DropColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_CardId");
            DropColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_PartnerTypeId");
            DropColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_Via");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_Via", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_PartnerTypeId", c => c.String(maxLength: 2, unicode: false));
            AddColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_CardId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_ContactId", c => c.String(maxLength: 15, unicode: false));
            DropForeignKey("dbo.SharedLogisticsContactLastLogins", "ContactId", "dbo.Contacts");
            DropIndex("dbo.SharedLogisticsContactLastLogins", new[] { "ContactId" });
            CreateIndex("dbo.Contacts", new[] { "SharedLogisticsContactLastLogin_ContactId", "SharedLogisticsContactLastLogin_CardId", "SharedLogisticsContactLastLogin_PartnerTypeId", "SharedLogisticsContactLastLogin_Via" });
            AddForeignKey("dbo.Contacts", new[] { "SharedLogisticsContactLastLogin_ContactId", "SharedLogisticsContactLastLogin_CardId", "SharedLogisticsContactLastLogin_PartnerTypeId", "SharedLogisticsContactLastLogin_Via" }, "dbo.SharedLogisticsContactLastLogins", new[] { "ContactId", "CardId", "PartnerTypeId", "Via" });
        }
    }
}
