namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missingmigrations29032020 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
               "dbo.TariffSurchargesUpdateMethods",
               c => new
               {
                   Code = c.String(nullable: false, maxLength: 3, unicode: false),
                   Name = c.String(maxLength: 40, unicode: false),
                   SearchFields = c.String(),
               })
               .PrimaryKey(t => t.Code);
        }
        
        public override void Down()
        {
        }
    }
}
