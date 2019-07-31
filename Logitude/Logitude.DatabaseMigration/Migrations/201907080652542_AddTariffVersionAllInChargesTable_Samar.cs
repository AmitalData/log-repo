namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTariffVersionAllInChargesTable_Samar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TariffVersionAllInCharges",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        Version = c.Int(nullable: false),
                        ChargesTypeId = c.String(nullable: false, maxLength: 15, unicode: false),
                        AddedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        AddDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AddedByUserId)
                .ForeignKey("dbo.ChargesTypes", t => t.ChargesTypeId)
                .Index(t => t.ChargesTypeId)
                .Index(t => t.AddedByUserId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TariffVersionAllInCharges", "ChargesTypeId", "dbo.ChargesTypes");
            DropForeignKey("dbo.TariffVersionAllInCharges", "AddedByUserId", "dbo.Users");
            DropIndex("dbo.TariffVersionAllInCharges", new[] { "AddedByUserId" });
            DropIndex("dbo.TariffVersionAllInCharges", new[] { "ChargesTypeId" });
            DropTable("dbo.TariffVersionAllInCharges");
        }
    }
}
