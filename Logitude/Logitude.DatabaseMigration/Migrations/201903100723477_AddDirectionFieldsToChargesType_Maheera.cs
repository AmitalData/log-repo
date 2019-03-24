namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDirectionFieldsToChargesType_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChargesTypes", "IsImport", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChargesTypes", "IsDomestic", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChargesTypes", "IsExport", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChargesTypes", "IsDrop", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChargesTypes", "IsDrop");
            DropColumn("dbo.ChargesTypes", "IsExport");
            DropColumn("dbo.ChargesTypes", "IsDomestic");
            DropColumn("dbo.ChargesTypes", "IsImport");
        }
    }
}
