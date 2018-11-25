namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MissingGlobalsSinceR4_Rabaia : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.CaptchaKeys",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 40, unicode: false),
            //            Code = c.String(nullable: false, maxLength: 6, unicode: false),
            //            CreateDate = c.DateTime(nullable: false),
            //            Email = c.String(maxLength: 70, unicode: false),
            //            IP = c.String(maxLength: 15, unicode: false),
            //            Activity = c.String(maxLength: 70, unicode: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.InvalidEmailResetPasswords",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            IP = c.String(maxLength: 15, unicode: false),
            //            CreateDate = c.DateTime(nullable: false),
            //            Email = c.String(maxLength: 70, unicode: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.SessionPolicies",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            WebTokenLifeTimeInMinutes = c.Int(nullable: false),
            //            WebTokenExpirationWarningInMinutes = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //AddColumn("dbo.TenantManagements", "StockTypeCode", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.GlobalDBs", "SharedDWConnection", c => c.String(nullable: false, maxLength: 512));
            //AddColumn("dbo.ChangePasswordLogs", "IP", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.Settings", "DWNextRunTime", c => c.DateTime());
            //AlterColumn("dbo.TenantManagements", "Notes", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TenantManagements", "Notes", c => c.String(maxLength: 250));
            DropColumn("dbo.Settings", "DWNextRunTime");
            DropColumn("dbo.ChangePasswordLogs", "IP");
            DropColumn("dbo.GlobalDBs", "SharedDWConnection");
            DropColumn("dbo.TenantManagements", "StockTypeCode");
            DropTable("dbo.SessionPolicies");
            DropTable("dbo.InvalidEmailResetPasswords");
            DropTable("dbo.CaptchaKeys");
        }
    }
}
