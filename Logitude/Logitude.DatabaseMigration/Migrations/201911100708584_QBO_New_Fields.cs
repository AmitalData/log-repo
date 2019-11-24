namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QBO_New_Fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountingSettings", "RefreshToken", c => c.String(maxLength: 2000));
            AddColumn("dbo.AccountingSettings", "QBOOAuth", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountingSettings", "QBOOAuth");
            DropColumn("dbo.AccountingSettings", "RefreshToken");
        }
    }
}
