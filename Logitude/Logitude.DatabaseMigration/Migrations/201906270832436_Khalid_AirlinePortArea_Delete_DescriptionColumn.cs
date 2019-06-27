namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Khalid_AirlinePortArea_Delete_DescriptionColumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AirlineAreasPorts", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AirlineAreasPorts", "Description", c => c.String(nullable: true));    
        }
    }
}
