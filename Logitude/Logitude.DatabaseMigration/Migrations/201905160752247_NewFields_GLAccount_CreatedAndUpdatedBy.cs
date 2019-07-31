namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewFields_GLAccount_CreatedAndUpdatedBy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GLAccounts", "CreatedByUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.GLAccounts", "UpdatedByUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.GLAccounts", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.GLAccounts", "UpdateDate", c => c.DateTime(nullable: false));
            //AddColumn("dbo.TasksScheduler", "Retries", c => c.Int(nullable: false));
            CreateIndex("dbo.GLAccounts", "CreatedByUserId");
            CreateIndex("dbo.GLAccounts", "UpdatedByUserId");
            AddForeignKey("dbo.GLAccounts", "CreatedByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.GLAccounts", "UpdatedByUserId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GLAccounts", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.GLAccounts", "CreatedByUserId", "dbo.Users");
            DropIndex("dbo.GLAccounts", new[] { "UpdatedByUserId" });
            DropIndex("dbo.GLAccounts", new[] { "CreatedByUserId" });
            //DropColumn("dbo.TasksScheduler", "Retries");
            DropColumn("dbo.GLAccounts", "UpdateDate");
            DropColumn("dbo.GLAccounts", "CreateDate");
            DropColumn("dbo.GLAccounts", "UpdatedByUserId");
            DropColumn("dbo.GLAccounts", "CreatedByUserId");
        }
    }
}
