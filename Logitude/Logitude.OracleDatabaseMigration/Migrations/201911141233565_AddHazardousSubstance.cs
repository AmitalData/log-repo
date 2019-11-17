namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHazardousSubstance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.HazardousSubstances",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 3, unicode: false),
                        Name = c.String(maxLength: 3, unicode: false),
                        SearchFields = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            AlterColumn("Customs.ConsignmentPackDangers", "UNCode", c => c.String(maxLength: 4));
        }
        
        public override void Down()
        {
            AlterColumn("Customs.ConsignmentPackDangers", "UNCode", c => c.String(maxLength: 4, unicode: false));
            DropTable("Customs.HazardousSubstances");
        }
    }
}
