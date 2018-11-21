namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OpenFormatUpdatedByUserMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OpenFormatReports", "UpdatedByUserId", "dbo.Users");
            DropIndex("dbo.OpenFormatReports", new[] { "UpdatedByUserId" });
            DropColumn("dbo.OpenFormatReports", "UpdatedByUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OpenFormatReports", "UpdatedByUserId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            CreateIndex("dbo.OpenFormatReports", "UpdatedByUserId");
            AddForeignKey("dbo.OpenFormatReports", "UpdatedByUserId", "dbo.Users", "Id");
        }
    }
}
