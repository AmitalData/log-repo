namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeFieldsDiamonds : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.Declarations", "IsValidTicketsDiamond", c => c.Boolean(nullable: false));
            DropColumn("Customs.Declarations", "DocumentStatusDiamond");
            DropColumn("Customs.Declarations", "IsSignDiamond");
        }
        
        public override void Down()
        {
            AddColumn("Customs.Declarations", "IsSignDiamond", c => c.Boolean(nullable: false));
            AddColumn("Customs.Declarations", "DocumentStatusDiamond", c => c.String(maxLength: 3));
            DropColumn("Customs.Declarations", "IsValidTicketsDiamond");
        }
    }
}
