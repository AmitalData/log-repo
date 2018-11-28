namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddNewFieldIPToChangePasswordLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChangePasswordLogs", "IP", c => c.String(maxLength: 15, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChangePasswordLogs", "IP");
        }
    }
}
