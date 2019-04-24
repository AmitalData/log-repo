namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddFieldCodeToAutomation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Automations", "Code", c => c.String(maxLength: 7, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Automations", "Code");
        }
    }
}
