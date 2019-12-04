namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewField_AllowEditChequePayToName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GLAccounts", "AllowEditChequePayToName", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GLAccounts", "AllowEditChequePayToName");
        }
    }
}
