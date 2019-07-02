namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewField_FullAccountingSetting_GLAccountCounterLength : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FullAccountingSettings", "GLAccounterCounterLength", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FullAccountingSettings", "GLAccounterCounterLength");
        }
    }
}
