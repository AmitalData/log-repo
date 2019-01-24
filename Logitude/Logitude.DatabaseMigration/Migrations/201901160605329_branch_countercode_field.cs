namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class branch_countercode_field : DbMigration
    {
        public override void Up()
        {
			AddColumn("dbo.Branches", "CounterCode", c => c.String(maxLength: 5, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Branches", "CounterCode");
		}
    }
}
