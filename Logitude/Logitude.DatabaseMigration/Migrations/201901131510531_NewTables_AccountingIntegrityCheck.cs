namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTables_AccountingIntegrityCheck : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountingIntegrityChecks",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDateTimeUTC = c.DateTime(nullable: false),
                        StatusCode = c.String(maxLength: 1, unicode: false),
                        ParametersXML = c.String(maxLength: 1000),
                        ResultXML = c.String(),
                        HasException = c.Boolean(nullable: false, defaultValue: false),
                        DoneDateTimeUTC = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IntegrityCheckStatuses", t => t.StatusCode)
                .Index(t => t.StatusCode);
            
            CreateTable(
                "dbo.IntegrityCheckStatuses",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 1, unicode: false),
                        Name = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccountingIntegrityChecks", "StatusCode", "dbo.IntegrityCheckStatuses");
            DropIndex("dbo.AccountingIntegrityChecks", new[] { "StatusCode" });
            DropTable("dbo.IntegrityCheckStatuses");
            DropTable("dbo.AccountingIntegrityChecks");
        }
    }
}
