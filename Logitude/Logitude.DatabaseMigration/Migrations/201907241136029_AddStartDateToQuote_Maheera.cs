namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStartDateToQuote_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quotes", "StartDate", c => c.DateTime(nullable: true));
            Sql("Update Quotes set StartDate = dateadd(d,- ExpirationDays, ExpirationDate)");
        }

        public override void Down()
        {
            DropColumn("dbo.Quotes", "StartDate");
        }
    }
}
