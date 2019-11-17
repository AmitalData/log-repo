namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeHazardousSubstances : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("Customs.HazardousSubstances");
            AlterColumn("Customs.ConsignmentPackDangers", "UNCode", c => c.String(nullable: false, maxLength: 4, unicode: false));
            AlterColumn("Customs.HazardousSubstances", "Code", c => c.String(nullable: false, maxLength: 4, unicode: false));
            AlterColumn("Customs.HazardousSubstances", "Name", c => c.String(maxLength: 100, unicode: false));
            AddPrimaryKey("Customs.HazardousSubstances", "Code");
            CreateIndex("Customs.ConsignmentPackDangers", "UNCode");
            AddForeignKey("Customs.ConsignmentPackDangers", "UNCode", "Customs.HazardousSubstances", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.ConsignmentPackDangers", "UNCode", "Customs.HazardousSubstances");
            DropIndex("Customs.ConsignmentPackDangers", new[] { "UNCode" });
            DropPrimaryKey("Customs.HazardousSubstances");
            AlterColumn("Customs.HazardousSubstances", "Name", c => c.String(maxLength: 3, unicode: false));
            AlterColumn("Customs.HazardousSubstances", "Code", c => c.String(nullable: false, maxLength: 3, unicode: false));
            AlterColumn("Customs.ConsignmentPackDangers", "UNCode", c => c.String(maxLength: 4));
            AddPrimaryKey("Customs.HazardousSubstances", "Code");
        }
    }
}
