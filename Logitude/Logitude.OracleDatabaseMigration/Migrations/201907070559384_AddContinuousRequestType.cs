namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContinuousRequestType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.ContinuousRequestTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 2, unicode: false),
                        LocalName = c.String(maxLength: 40),
                        SearchFields = c.String(maxLength: 1000),
                        EnglishName = c.String(maxLength: 40, unicode: false),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropTable("Customs.ContinuousRequestTypes");
        }
    }
}
