namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewField_ReconciliationLine_ReconciledWithTransactionId : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Users", "ShowNewReleaseToolTip", c => c.Boolean(nullable: false));
            AddColumn("dbo.ReconciliationLines", "ReconciledWithTransactionId", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.TaskSchedulerHistory", "StartDateTimeUTC", c => c.DateTime());
            //AddColumn("dbo.TaskSchedulerHistory", "EndDateTimeUTC", c => c.DateTime());
            //AddColumn("dbo.TasksScheduler", "NextRunTimeUTC", c => c.DateTime());
            //AddColumn("dbo.TasksScheduler", "LastRunTimeUTC", c => c.DateTime());
            //AddColumn("dbo.TasksScheduler", "StartDateTimeUTC", c => c.DateTime());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.TasksScheduler", "StartDateTimeUTC");/
            //DropColumn("dbo.TasksScheduler", "LastRunTimeUTC");
            //DropColumn("dbo.TasksScheduler", "NextRunTimeUTC");
            //DropColumn("dbo.TaskSchedulerHistory", "EndDateTimTC");
            //DropColumn("dbo.TaskSchedulerHistory", "StartDateTi/meUTC");
            DropColumn("dbo.ReconciliationLines", "ReconciledWithTransactionId");
            //DropColumn("dbo.Users", "ShowNewReleaseToolTip");
        }
    }
}
