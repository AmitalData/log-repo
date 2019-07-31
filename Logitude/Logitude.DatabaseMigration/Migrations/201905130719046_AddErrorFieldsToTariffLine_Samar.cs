namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddErrorFieldsToTariffLine_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TariffLines", "HasErrors", c => c.Boolean(nullable: false));
            AddColumn("dbo.TariffLines", "ErrorText", c => c.String(maxLength: 500, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TariffLines", "ErrorText");
            DropColumn("dbo.TariffLines", "HasErrors");
        }
    }
}
