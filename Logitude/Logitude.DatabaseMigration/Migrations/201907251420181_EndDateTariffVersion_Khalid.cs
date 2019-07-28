namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EndDateTariffVersion_Khalid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TariffVersions", "InitialEnddate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TariffVersions", "InitialEnddate");
        }
    }
}
