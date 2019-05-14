namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddFieldIsCustomToDWObjectField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DWObjectFields", "IsCustom", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DWObjectFields", "IsCustom");
        }
    }
}
