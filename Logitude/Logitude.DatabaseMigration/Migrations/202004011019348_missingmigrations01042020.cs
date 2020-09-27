namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missingmigrations01042020 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.ConsignmentPackDangers", "ClassificationFourDigit", c => c.String(maxLength: 4));
        }
        
        public override void Down()
        {
        }
    }
}
