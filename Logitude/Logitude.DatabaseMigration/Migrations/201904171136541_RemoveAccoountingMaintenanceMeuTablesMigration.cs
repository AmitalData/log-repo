namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAccoountingMaintenanceMeuTablesMigration : DbMigration
    {
        public override void Up()
        {
            Sql("delete from MenusTables where Code='TXRP'");
            Sql("delete from MenusTables where Code='TXDR'");
            Sql("delete from MenusTables where Code='MTRV'");

        }
        
        public override void Down()
        {
        }
    }
}
