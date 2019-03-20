namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingCantFilterAndHelpTextToDWObjectFields_Rabaia : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.DWObjectFields", "CannotFilter", c => c.Boolean(nullable: false));
            //AddColumn("dbo.DWObjectFields", "HelpText", c => c.String(maxLength: 2000, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DWObjectFields", "HelpText");
            DropColumn("dbo.DWObjectFields", "CannotFilter");
        }
    }
}
