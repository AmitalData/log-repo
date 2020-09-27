namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomsShip : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.CustomsShips",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 4, unicode: false),
                        EnglishName = c.String(maxLength: 300, unicode: false),
                        SearchFields = c.String(),
                        LocalName = c.String(maxLength: 300),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropTable("Customs.CustomsShips");
        }
    }
}
