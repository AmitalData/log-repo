namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RferantChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Customs.DeclarationReferantDatas", "ArrivalDate", c => c.DateTime(precision: 7));
            AlterColumn("Customs.DeclarationReferantDatas", "EstimatedArrivalDate", c => c.DateTime(precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("Customs.DeclarationReferantDatas", "EstimatedArrivalDate", c => c.DateTime(nullable: false, precision: 7));
            AlterColumn("Customs.DeclarationReferantDatas", "ArrivalDate", c => c.DateTime(nullable: false, precision: 7));
        }
    }
}
