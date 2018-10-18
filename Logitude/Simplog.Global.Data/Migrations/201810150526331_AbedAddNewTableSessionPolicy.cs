namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddNewTableSessionPolicy : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SessionPolicies",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        WebTokenLifeTime = c.Int(nullable: false),
                        WebTokenExpirationWarning = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SessionPolicies");
        }
    }
}
