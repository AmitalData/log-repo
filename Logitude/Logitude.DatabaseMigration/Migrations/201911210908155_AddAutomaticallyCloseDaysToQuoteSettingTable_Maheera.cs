namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAutomaticallyCloseDaysToQuoteSettingTable_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuoteSettings", "AutomaticallyCloseDays", c => c.Int(nullable: false));
            Sql("update QuoteSettings set AutomaticallyCloseDays ='30'");
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuoteSettings", "AutomaticallyCloseDays");
        }
    }
}
