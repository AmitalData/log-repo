namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JournalIsExternalLineFieldMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaxReportLines", "IsExternalLine", c => c.Boolean(nullable: false));

        }
        
        public override void Down()
        {
            DropColumn("dbo.TaxReportLines", "IsExternalLine");
        }
    }
}
