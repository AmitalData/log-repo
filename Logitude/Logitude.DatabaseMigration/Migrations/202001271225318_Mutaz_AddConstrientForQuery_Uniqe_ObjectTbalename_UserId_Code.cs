namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_AddConstrientForQuery_Uniqe_ObjectTbalename_UserId_Code : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE Queries ADD CONSTRAINT[UQ_ObjectTableId_UserId_UniqueCode] UNIQUE NONCLUSTERED ([ObjectTableId] ASC,[UserId] ASC,[UniqueCode] ASC)");
        }
        
        public override void Down()
        {
        }
    }
}
