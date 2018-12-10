namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigrationPendingErrorPlace : DbMigration
    {
        public override void Up()
        {
            //CreateIndex("Customs.CourierPendingReasons", "ErrorPlace");
            //AddForeignKey("Customs.CourierPendingReasons", "ErrorPlace", "Customs.PendingErrorPlaces", "Code");
        }
        
        public override void Down()
        {
            //DropForeignKey("Customs.CourierPendingReasons", "ErrorPlace", "Customs.PendingErrorPlaces");
            //DropIndex("Customs.CourierPendingReasons", new[] { "ErrorPlace" });
        }
    }
}
