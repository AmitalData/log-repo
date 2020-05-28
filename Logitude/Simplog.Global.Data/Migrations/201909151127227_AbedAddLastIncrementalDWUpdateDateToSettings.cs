namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddLastIncrementalDWUpdateDateToSettings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "LastIncrementalDWUpdateDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "LastIncrementalDWUpdateDate");
        }
    }
}
