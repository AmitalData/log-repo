namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewFieldsToTariffVersionLevel_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TariffVersions", "IsDraft", c => c.Boolean(nullable: false));
            AddColumn("dbo.TariffVersions", "ApproveDate", c => c.DateTime());
            AddColumn("dbo.TariffVersions", "ApprovedByUserId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.TariffVersions", "ApprovedByUserId");
            AddForeignKey("dbo.TariffVersions", "ApprovedByUserId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TariffVersions", "ApprovedByUserId", "dbo.Users");
            DropIndex("dbo.TariffVersions", new[] { "ApprovedByUserId" });
            DropColumn("dbo.TariffVersions", "ApprovedByUserId");
            DropColumn("dbo.TariffVersions", "ApproveDate");
            DropColumn("dbo.TariffVersions", "IsDraft");
        }
    }
}
