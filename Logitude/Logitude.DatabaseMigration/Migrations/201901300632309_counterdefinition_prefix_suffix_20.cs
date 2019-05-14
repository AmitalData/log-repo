namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class counterdefinition_prefix_suffix_20 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CounterDefinitions", "Prefix", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.CounterDefinitions", "Suffix", c => c.String(maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CounterDefinitions", "Suffix", c => c.String(maxLength: 10, unicode: false));
            AlterColumn("dbo.CounterDefinitions", "Prefix", c => c.String(maxLength: 10, unicode: false));
        }
    }
}
