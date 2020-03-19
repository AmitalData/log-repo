namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTariffProductsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TariffProducts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        Code = c.String(nullable: false, maxLength: 3, unicode: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        LocalName = c.String(maxLength: 100),
                        Inactive = c.Boolean(nullable: false),
                        SearchFields = c.String(),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.TariffProducts");
        }
    }
}
