namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetOwnerNotRequiredInOwnerHistory_Samar : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ActivityOwnerHistories", new[] { "OwnerId" });
            AlterColumn("dbo.ActivityOwnerHistories", "OwnerId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.ActivityOwnerHistories", "OwnerId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ActivityOwnerHistories", new[] { "OwnerId" });
            AlterColumn("dbo.ActivityOwnerHistories", "OwnerId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            CreateIndex("dbo.ActivityOwnerHistories", "OwnerId");
        }
    }
}
