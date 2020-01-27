namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_Re_FillUniqueCodeInQuery : DbMigration
    {
        public override void Up()
        {
            Sql("update Queries set UniqueCode = (select ObjectTables.Name from ObjectTables where Id= Queries.ObjectTableId)+'.'+Queries.UserId+'.'+Queries.Code where Tenant != 0 ;");
            Sql("update Queries set UniqueCode = (select ObjectTables.Name from ObjectTables where Id= Queries.ObjectTableId)+'.'+Queries.Code where Tenant = 0 ;");
            Sql("ALTER TABLE Queries ALTER COLUMN UniqueCode VARCHAR(200) NOT NULL;");

        }

        public override void Down()
        { 
        }
    }
}
