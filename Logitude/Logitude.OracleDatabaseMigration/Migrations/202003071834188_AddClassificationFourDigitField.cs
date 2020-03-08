namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClassificationFourDigitField : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.ConsignmentPackDangers", "ClassificationFourDigit", c => c.String(maxLength: 4));
        }
        
        public override void Down()
        {
            DropColumn("Customs.ConsignmentPackDangers", "ClassificationFourDigit");
        }
    }
}
