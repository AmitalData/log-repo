namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeTariffVersionDatesNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TariffVersions", "StartDate", c => c.DateTime());
            AlterColumn("dbo.TariffVersions", "ExpirationDate", c => c.DateTime());

            Sql("update TariffVersions set StartDate = null, ExpirationDate = null where TariffId in ( select Id from Tariffs where TypeCode = 'ASC')");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TariffVersions", "ExpirationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TariffVersions", "StartDate", c => c.DateTime(nullable: false));
        }
    }
}
