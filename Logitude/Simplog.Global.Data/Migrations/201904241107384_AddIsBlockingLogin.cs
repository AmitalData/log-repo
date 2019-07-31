namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsBlockingLogin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GlobalDBs", "IsBlocking", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GlobalDBs", "IsBlocking");
        }
    }
}
