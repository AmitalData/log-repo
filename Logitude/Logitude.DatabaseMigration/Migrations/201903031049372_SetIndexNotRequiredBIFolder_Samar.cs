namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetIndexNotRequiredBIFolder_Samar : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BIReportFolders", "Index", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BIReportFolders", "Index", c => c.Int(nullable: false));
        }
    }
}
