namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CardFieldsMappingMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cards", "BankName", c => c.String(maxLength: 40));
            AlterColumn("dbo.Cards", "BankAddress", c => c.String(maxLength: 100));
            //AlterColumn("dbo.JournalLines", "Notes", c => c.String(maxLength: 250));
            //DropColumn("dbo.CustomsTransferLines", "SearchFields");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomsTransferLines", "SearchFields", c => c.String(maxLength: 1000));
            AlterColumn("dbo.JournalLines", "Notes", c => c.String(maxLength: 60));
            AlterColumn("dbo.Cards", "BankAddress", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.Cards", "BankName", c => c.String(maxLength: 40, unicode: false));
        }
    }
}
