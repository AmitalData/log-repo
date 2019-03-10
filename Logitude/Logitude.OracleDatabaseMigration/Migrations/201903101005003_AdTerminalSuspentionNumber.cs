namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdTerminalSuspentionNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.DeclarationCourierStatuses", "TerminalSuspentionNumber", c => c.String(maxLength: 6, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("Customs.DeclarationCourierStatuses", "TerminalSuspentionNumber");
        }
    }
}
