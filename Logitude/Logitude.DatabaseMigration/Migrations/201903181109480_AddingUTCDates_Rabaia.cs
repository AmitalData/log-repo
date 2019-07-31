namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingUTCDates_Rabaia : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.NumberFormats",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 128),
            //            Name = c.String(),
            //            SearchFields = c.String(),
            //        })
            //    .PrimaryKey(t => t.Code);
            
            //AddColumn("dbo.Tenants", "NumberFormatCode", c => c.String(maxLength: 128));
            //AddColumn("dbo.ChargesTypes", "IsImport", c => c.Boolean(nullable: false));
            //AddColumn("dbo.ChargesTypes", "IsDomestic", c => c.Boolean(nullable: false));
            //AddColumn("dbo.ChargesTypes", "IsExport", c => c.Boolean(nullable: false));
            //AddColumn("dbo.ChargesTypes", "IsDrop", c => c.Boolean(nullable: false));
            //AddColumn("dbo.ARPayments", "IsFullAccounting", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Shipments", "ARInvoices", c => c.String(maxLength: 1000, unicode: false));
            //AddColumn("dbo.ReconciliationLines", "SearchFields", c => c.String());
            AddColumn("dbo.TaskSchedulerHistory", "StartDateTimeUTC", c => c.DateTime());
            AddColumn("dbo.TaskSchedulerHistory", "EndDateTimeUTC", c => c.DateTime());
            //AddColumn("dbo.TasksScheduler", "SchedulerDetailsXML", c => c.String());
            //AddColumn("dbo.TasksScheduler", "Type", c => c.String(maxLength: 15, unicode: false));
            //AlterColumn("dbo.ReconcileExternalPageLines", "Reference", c => c.String(maxLength: 30));
            //AlterColumn("dbo.Reconciliations", "SearchFields", c => c.String());
            //CreateIndex("dbo.Tenants", "NumberFormatCode");
            //AddForeignKey("dbo.Tenants", "NumberFormatCode", "dbo.NumberFormats", "Code");
            //DropColumn("dbo.TMBudgets", "Inactive");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.TMBudgets", "Inactive", c => c.Boolean(nullable: false));
            //DropForeignKey("dbo.Tenants", "NumberFormatCode", "dbo.NumberFormats");
            //DropIndex("dbo.Tenants", new[] { "NumberFormatCode" });
            //AlterColumn("dbo.Reconciliations", "SearchFields", c => c.String(maxLength: 1000));
            //AlterColumn("dbo.ReconcileExternalPageLines", "Reference", c => c.String(maxLength: 30, unicode: false));
            //DropColumn("dbo.TasksScheduler", "Type");
            //DropColumn("dbo.TasksScheduler", "SchedulerDetailsXML");
            DropColumn("dbo.TaskSchedulerHistory", "EndDateTimeUTC");
            DropColumn("dbo.TaskSchedulerHistory", "StartDateTimeUTC");
            //DropColumn("dbo.ReconciliationLines", "SearchFields");
            //DropColumn("dbo.Shipments", "ARInvoices");
            //DropColumn("dbo.ARPayments", "IsFullAccounting");
            //DropColumn("dbo.ChargesTypes", "IsDrop");
            //DropColumn("dbo.ChargesTypes", "IsExport");
            //DropColumn("dbo.ChargesTypes", "IsDomestic");
            //DropColumn("dbo.ChargesTypes", "IsImport");
            //DropColumn("dbo.Tenants", "NumberFormatCode");
            //DropTable("dbo.NumberFormats");
        }
    }
}
