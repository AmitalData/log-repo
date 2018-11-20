namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMogrationDecisionType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.DecisionTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 2, unicode: false),
                        LocalName = c.String(maxLength: 50),
                        EnglishName = c.String(maxLength: 50, unicode: false),
                        Inactive = c.Boolean(nullable: false),
                        SearchFields = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropTable("Customs.DecisionTypes");
        }
    }
}
