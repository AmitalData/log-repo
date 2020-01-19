namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigrationSealType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.SealTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 3, unicode: false),
                        LocalName = c.String(maxLength: 40),
                        EnglishName = c.String(maxLength: 40),
                        SearchFields = c.String(maxLength: 1000),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropTable("Customs.SealTypes");
        }
    }
}
