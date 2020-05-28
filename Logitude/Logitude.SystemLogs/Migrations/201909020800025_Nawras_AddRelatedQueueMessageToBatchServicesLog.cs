namespace Logitude.SystemLogs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nawras_AddRelatedQueueMessageToBatchServicesLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BatchServicesLogs", "RelatedQueueMessage", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BatchServicesLogs", "RelatedQueueMessage");
        }
    }
}
