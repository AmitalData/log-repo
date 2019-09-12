namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VatTypeRecognizedPercentageMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VatTypes", "RecognizedPercentage", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VatTypes", "RecognizedPercentage");
        }
    }
}
