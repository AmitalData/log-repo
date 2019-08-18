namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAMANACStartDatesToCustomsInterfaceSettings_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomsInterfaceSettings", "AMCAirStartDate", c => c.DateTime());
            AddColumn("dbo.CustomsInterfaceSettings", "AMCOceanStartDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomsInterfaceSettings", "AMCOceanStartDate");
            DropColumn("dbo.CustomsInterfaceSettings", "AMCAirStartDate");
        }
    }
}
