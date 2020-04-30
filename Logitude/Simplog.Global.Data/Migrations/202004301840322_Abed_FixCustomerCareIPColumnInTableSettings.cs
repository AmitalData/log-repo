namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Abed_FixCustomerCareIPColumnInTableSettings : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Settings", "CustomerCareIP", c => c.String(nullable: false, maxLength: 250, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Settings", "CustomerCareIP", c => c.String(nullable: false, maxLength: 100, unicode: false));
        }
    }
}
