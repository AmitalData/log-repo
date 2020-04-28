namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLastUsedDateToTariff_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tariffs", "LastUsedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tariffs", "LastUsedDate");
        }
    }
}
