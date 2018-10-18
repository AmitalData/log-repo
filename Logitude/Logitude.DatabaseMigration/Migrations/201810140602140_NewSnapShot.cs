namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewSnapShot : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.CustomsAirlines",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        AirlineCode = c.String(nullable: false, maxLength: 2, unicode: false),
                        LocalName = c.String(maxLength: 100),
                        EnglishName = c.String(maxLength: 70, unicode: false),
                        InActive = c.Boolean(nullable: false),
                        SearchFields = c.String(maxLength: 500),
                        AirlinePrefix = c.String(nullable: false, maxLength: 3, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
           
            DropTable("Customs.CustomsAirlines");
            
        }
    }
}
