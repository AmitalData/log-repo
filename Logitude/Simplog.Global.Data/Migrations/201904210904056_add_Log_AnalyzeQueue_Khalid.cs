namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_Log_AnalyzeQueue_Khalid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnalyzeQueues", "Log", c => c.String(maxLength: 500, unicode: false));
        }
        
        public override void Down()
        {      
            DropColumn("dbo.AnalyzeQueues", "Log");
        }
    }
}
