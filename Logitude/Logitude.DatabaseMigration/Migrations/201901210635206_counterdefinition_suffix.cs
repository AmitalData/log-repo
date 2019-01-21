namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class counterdefinition_suffix : DbMigration
    {
        public override void Up()
        {
           
            AddColumn("dbo.CounterDefinitions", "Suffix", c => c.String(maxLength: 10, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CounterDefinitions", "Suffix");
            
        }
    }
}
