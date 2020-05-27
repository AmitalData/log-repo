namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingQueueDefCOdeToBatchService_Rabaia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BatchServicesDefinitions", "QueueDefinitionCode", c => c.String(maxLength:200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BatchServicesDefinitions", "QueueDefinitionCode");
        }
    }
}
