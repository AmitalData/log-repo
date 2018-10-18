namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhysicalCheckSpotlightMigration : DbMigration
    {
        public override void Up()
        {
            Sql("update Queries set SpotlightDataTemplate= null where ObjectTableId =(select ID from ObjectTables where Name='customs.physicalcheck')");
        }
        
        public override void Down()
        {
        }
    }
}
