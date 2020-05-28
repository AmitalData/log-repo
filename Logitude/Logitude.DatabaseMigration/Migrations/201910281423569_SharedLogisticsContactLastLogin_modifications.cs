namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SharedLogisticsContactLastLogin_modifications : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SharedLogisticsContactLastLogins", "Id", "dbo.Contacts");
            RenameColumn(table: "dbo.SharedLogisticsContactLastLogins", name: "Id", newName: "ContactId");
            RenameIndex(table: "dbo.SharedLogisticsContactLastLogins", name: "IX_Id", newName: "IX_ContactId");
            DropPrimaryKey("dbo.SharedLogisticsContactLastLogins");
            AddColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_ContactId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_CardId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_PartnerTypeId", c => c.String(maxLength: 2, unicode: false));
            AddColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_Via", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.SharedLogisticsContactLastLogins", "Via", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AddPrimaryKey("dbo.SharedLogisticsContactLastLogins", new[] { "ContactId", "CardId", "PartnerTypeId", "Via" });
            CreateIndex("dbo.Contacts", new[] { "SharedLogisticsContactLastLogin_ContactId", "SharedLogisticsContactLastLogin_CardId", "SharedLogisticsContactLastLogin_PartnerTypeId", "SharedLogisticsContactLastLogin_Via" });
            AddForeignKey("dbo.Contacts", new[] { "SharedLogisticsContactLastLogin_ContactId", "SharedLogisticsContactLastLogin_CardId", "SharedLogisticsContactLastLogin_PartnerTypeId", "SharedLogisticsContactLastLogin_Via" }, "dbo.SharedLogisticsContactLastLogins", new[] { "ContactId", "CardId", "PartnerTypeId", "Via" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contacts", new[] { "SharedLogisticsContactLastLogin_ContactId", "SharedLogisticsContactLastLogin_CardId", "SharedLogisticsContactLastLogin_PartnerTypeId", "SharedLogisticsContactLastLogin_Via" }, "dbo.SharedLogisticsContactLastLogins");
            DropIndex("dbo.Contacts", new[] { "SharedLogisticsContactLastLogin_ContactId", "SharedLogisticsContactLastLogin_CardId", "SharedLogisticsContactLastLogin_PartnerTypeId", "SharedLogisticsContactLastLogin_Via" });
            DropPrimaryKey("dbo.SharedLogisticsContactLastLogins");
            DropColumn("dbo.SharedLogisticsContactLastLogins", "Via");
            DropColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_Via");
            DropColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_PartnerTypeId");
            DropColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_CardId");
            DropColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_ContactId");
            AddPrimaryKey("dbo.SharedLogisticsContactLastLogins", "Id");
            RenameIndex(table: "dbo.SharedLogisticsContactLastLogins", name: "IX_ContactId", newName: "IX_Id");
            RenameColumn(table: "dbo.SharedLogisticsContactLastLogins", name: "ContactId", newName: "Id");
            AddForeignKey("dbo.SharedLogisticsContactLastLogins", "Id", "dbo.Contacts", "Id");
        }
    }
}
