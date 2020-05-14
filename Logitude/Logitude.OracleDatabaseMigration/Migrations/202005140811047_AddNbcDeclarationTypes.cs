namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNbcDeclarationTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.NbcDeclarationTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 4, unicode: false),
                        EnglishName = c.String(maxLength: 100, unicode: false),
                        LocalName = c.String(maxLength: 100),
                        SearchFields = c.String(maxLength: 1000),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
        }
        
        public override void Down()
        {
            DropTable("Customs.NbcDeclarationTypes");
        }
    }
}
