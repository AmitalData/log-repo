namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBIReportsDB_Maheera : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BIReports",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        SearchFields = c.String(),
                        Name = c.String(nullable: false, maxLength: 80),
                        Description = c.String(maxLength: 1000),
                        DWQueryId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Inactive = c.Boolean(nullable: false),
                        TypeCode = c.String(maxLength: 3, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BIReportsTypes", t => t.TypeCode)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.DWQueries", t => t.DWQueryId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.DWQueryId)
                .Index(t => t.TypeCode);
            
            CreateTable(
                "dbo.BIReportsTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 3, unicode: false),
                        Name = c.String(maxLength: 3, unicode: false),
                        SearchFields = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BIReports", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.BIReports", "DWQueryId", "dbo.DWQueries");
            DropForeignKey("dbo.BIReports", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.BIReports", "TypeCode", "dbo.BIReportsTypes");
            DropIndex("dbo.BIReports", new[] { "TypeCode" });
            DropIndex("dbo.BIReports", new[] { "DWQueryId" });
            DropIndex("dbo.BIReports", new[] { "UpdatedByUserId" });
            DropIndex("dbo.BIReports", new[] { "CreatedByUserId" });
            DropTable("dbo.BIReportsTypes");
            DropTable("dbo.BIReports");
        }
    }
}
