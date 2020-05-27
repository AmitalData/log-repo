namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nawras_Add_IsInternallyDefined_ToSchedulerProcedure : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.TariffVersions", "InitialEnddate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TariffVersions", "InitialEnddate");
        }
    }
}
