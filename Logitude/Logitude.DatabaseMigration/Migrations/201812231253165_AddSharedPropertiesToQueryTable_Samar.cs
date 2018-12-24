namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSharedPropertiesToQueryTable_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Queries", "SharedWithAll", c => c.Boolean(nullable: false));
            AddColumn("dbo.Queries", "SharedWithSpecificUsers", c => c.Boolean(nullable: false));
            AddColumn("dbo.Queries", "SharedByUserId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.Queries", "SharedByUserId");
            AddForeignKey("dbo.Queries", "SharedByUserId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Queries", "SharedByUserId", "dbo.Users");
            DropIndex("dbo.Queries", new[] { "SharedByUserId" });
            DropColumn("dbo.Queries", "SharedByUserId");
            DropColumn("dbo.Queries", "SharedWithSpecificUsers");
            DropColumn("dbo.Queries", "SharedWithAll");
        }
    }
}
