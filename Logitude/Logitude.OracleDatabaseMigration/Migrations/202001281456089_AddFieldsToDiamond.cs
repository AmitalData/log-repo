namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsToDiamond : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.Declarations", "IsMissMandatoryDiamond", c => c.Boolean(nullable: false));
            AddColumn("Customs.Declarations", "DocumentStatusDiamond", c => c.String(maxLength: 3));
            AddColumn("Customs.Declarations", "IsSignDiamond", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Customs.Declarations", "IsSignDiamond");
            DropColumn("Customs.Declarations", "DocumentStatusDiamond");
            DropColumn("Customs.Declarations", "IsMissMandatoryDiamond");
        }
    }
}
