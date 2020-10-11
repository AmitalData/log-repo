namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnabaleNullAvailabilityDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Customs.Declarations", "AvailabilityDate", c => c.DateTime(precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("Customs.Declarations", "AvailabilityDate", c => c.DateTime(nullable: false, precision: 7));
        }
    }
}
