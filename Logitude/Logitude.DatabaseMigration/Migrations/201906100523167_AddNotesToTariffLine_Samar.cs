namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotesToTariffLine_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TariffLines", "Notes", c => c.String(maxLength: 500));            
        }
        
        public override void Down()
        {
            DropColumn("dbo.TariffLines", "Notes");            
        }
    }
}
