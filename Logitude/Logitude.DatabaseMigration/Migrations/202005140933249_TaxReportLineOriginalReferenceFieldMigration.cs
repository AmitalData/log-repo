namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaxReportLineOriginalReferenceFieldMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaxReportLines", "OriginalReference", c => c.String(maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaxReportLines", "OriginalReference");
        }
    }
}
