namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCheckDigitControlAlgorithm : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CheckDigitControlAlgorithms",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 4, unicode: false),
                        Name = c.String(nullable: false, maxLength: 20, unicode: false),
                        SearchFields = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Code);
            
            AddColumn("dbo.Tenants", "CheckDigitControlAlgorithmCode", c => c.String(maxLength: 4, unicode: false));
            CreateIndex("dbo.Tenants", "CheckDigitControlAlgorithmCode");
            AddForeignKey("dbo.Tenants", "CheckDigitControlAlgorithmCode", "dbo.CheckDigitControlAlgorithms", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tenants", "CheckDigitControlAlgorithmCode", "dbo.CheckDigitControlAlgorithms");
            DropIndex("dbo.Tenants", new[] { "CheckDigitControlAlgorithmCode" });
            DropColumn("dbo.Tenants", "CheckDigitControlAlgorithmCode");
            DropTable("dbo.CheckDigitControlAlgorithms");
        }
    }
}
