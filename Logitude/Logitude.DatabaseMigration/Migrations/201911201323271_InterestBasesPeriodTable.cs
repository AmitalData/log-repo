namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InterestBasesPeriodTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InterestBasesPeriods",
                c => new
                    {
                        InterestBaseTypeId = c.String(nullable: false, maxLength: 15, unicode: false),
                        LineNumber = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(maxLength: 15, unicode: false),
                        InterestBaseStartDate = c.DateTime(nullable: false),
                        InterestRate = c.Decimal(nullable: false, precision: 4, scale: 2),
                    })
                .PrimaryKey(t => new { t.InterestBaseTypeId, t.LineNumber })
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.InterestBasesTypes", t => t.InterestBaseTypeId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.InterestBaseTypeId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InterestBasesPeriods", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.InterestBasesPeriods", "InterestBaseTypeId", "dbo.InterestBasesTypes");
            DropForeignKey("dbo.InterestBasesPeriods", "CreatedByUserId", "dbo.Users");
            DropIndex("dbo.InterestBasesPeriods", new[] { "UpdatedByUserId" });
            DropIndex("dbo.InterestBasesPeriods", new[] { "CreatedByUserId" });
            DropIndex("dbo.InterestBasesPeriods", new[] { "InterestBaseTypeId" });
            DropTable("dbo.InterestBasesPeriods");
        }
    }
}
