namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSealReasonTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.AmendmentTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 4, unicode: false),
                        Name = c.String(maxLength: 300, unicode: false),
                        SearchFields = c.String(),
                        LocalName = c.String(maxLength: 300),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "Customs.SealUpdateReasonTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 4, unicode: false),
                        Name = c.String(maxLength: 300, unicode: false),
                        SearchFields = c.String(),
                        LocalName = c.String(maxLength: 300),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropTable("Customs.SealUpdateReasonTypes");
            DropTable("Customs.AmendmentTypes");
        }
    }
}
