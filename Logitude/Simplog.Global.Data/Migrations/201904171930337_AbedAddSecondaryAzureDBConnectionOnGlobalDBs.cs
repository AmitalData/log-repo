namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddSecondaryAzureDBConnectionOnGlobalDBs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GlobalDBs", "SecondaryAzureDBConnection", c => c.String(nullable: false, maxLength: 512));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GlobalDBs", "SecondaryAzureDBConnection");
        }
    }
}
