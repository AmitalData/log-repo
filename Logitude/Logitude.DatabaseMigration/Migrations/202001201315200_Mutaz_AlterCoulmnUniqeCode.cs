namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_AlterCoulmnUniqeCode : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.Queries", "UniqueCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }

        public override void Down()
        {
        }
    }
}
