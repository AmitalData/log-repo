namespace Logitude.Global.OracleMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missingGlobalMigraions : DbMigration
    {
        public override void Up()
        {
            return;
            CreateTable(
                "dbo.CaptchaKeys",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 40, unicode: false),
                        Code = c.String(nullable: false, maxLength: 6, unicode: false),
                        CreateDate = c.DateTime(nullable: false, precision: 7),
                        Email = c.String(maxLength: 70, unicode: false),
                        IP = c.String(maxLength: 15, unicode: false),
                        Activity = c.String(maxLength: 70, unicode: false),
                        IsUsed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InvalidEmailResetPasswords",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        IP = c.String(maxLength: 15, unicode: false),
                        CreateDate = c.DateTime(nullable: false, precision: 7),
                        Email = c.String(maxLength: 70, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WebhookKeys",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        AccessKey = c.String(maxLength: 200, unicode: false),
                        Tenant = c.Int(nullable: false),
                        PartnerName = c.String(nullable: false, maxLength: 150, unicode: false),
                        InActive = c.Boolean(nullable: false),
                        CreatedByUserName = c.String(nullable: false, maxLength: 150, unicode: false),
                        CreateDate = c.DateTime(nullable: false, precision: 7),
                        UpdatedByUserName = c.String(nullable: false, maxLength: 150, unicode: false),
                        UpdateDate = c.DateTime(nullable: false, precision: 7),
                        Description = c.String(maxLength: 200, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TenantManagements", "IsINTTRAStockPrepaid", c => c.Boolean(nullable: false));
            AddColumn("dbo.TenantManagements", "PackageCodeSearchField", c => c.String(maxLength: 250));
            AddColumn("dbo.TenantManagements", "IsINTTRAOnlyDemo", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChangePasswordLogs", "IP", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ContactPasswords", "CaptchaKey", c => c.String(maxLength: 40, unicode: false));
            AddColumn("dbo.SessionPolicies", "WebTokenLifeTimeInMinutes", c => c.Int(nullable: false));
            AddColumn("dbo.SessionPolicies", "WebTokenExpirationWarningInMin", c => c.Int(nullable: false));
            AddColumn("dbo.Settings", "EmailSendingQuota", c => c.Int(nullable: false));
            AddColumn("dbo.Settings", "DWNextRunTime", c => c.DateTime(precision: 7));
            DropColumn("dbo.ApiCredintials", "ComputingPartnerId");
            DropColumn("dbo.SessionPolicies", "WebTokenLifeTime");
            DropColumn("dbo.SessionPolicies", "WebTokenExpirationWarning");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SessionPolicies", "WebTokenExpirationWarning", c => c.Int(nullable: false));
            AddColumn("dbo.SessionPolicies", "WebTokenLifeTime", c => c.Int(nullable: false));
            AddColumn("dbo.ApiCredintials", "ComputingPartnerId", c => c.String(maxLength: 15, unicode: false));
            DropColumn("dbo.Settings", "DWNextRunTime");
            DropColumn("dbo.Settings", "EmailSendingQuota");
            DropColumn("dbo.SessionPolicies", "WebTokenExpirationWarningInMinutes");
            DropColumn("dbo.SessionPolicies", "WebTokenLifeTimeInMinutes");
            DropColumn("dbo.ContactPasswords", "CaptchaKey");
            DropColumn("dbo.ChangePasswordLogs", "IP");
            DropColumn("dbo.TenantManagements", "IsINTTRAOnlyDemo");
            DropColumn("dbo.TenantManagements", "PackageCodeSearchField");
            DropColumn("dbo.TenantManagements", "IsINTTRAStockPrepaid");
            DropTable("dbo.WebhookKeys");
            DropTable("dbo.InvalidEmailResetPasswords");
            DropTable("dbo.CaptchaKeys");
        }
    }
}
