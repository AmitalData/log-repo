namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncreaseQuoteSubjectField : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Quotes", "Subject", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Quotes", "Subject", c => c.String(maxLength: 60));
        }
    }
}
