namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigrationSignedByUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.ProceduralFaults", "SignedByUserId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("Customs.ProceduralFaults", "SignedByUserId");
            AddForeignKey("Customs.ProceduralFaults", "SignedByUserId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.ProceduralFaults", "SignedByUserId", "dbo.Users");
            DropIndex("Customs.ProceduralFaults", new[] { "SignedByUserId" });
            DropColumn("Customs.ProceduralFaults", "SignedByUserId");
        }
    }
}
