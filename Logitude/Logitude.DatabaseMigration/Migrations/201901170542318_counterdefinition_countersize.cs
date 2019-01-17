namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class counterdefinition_countersize : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CounterDefinitions", "CounterSize", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CounterDefinitions", "CounterSize");
        }
    }
}
