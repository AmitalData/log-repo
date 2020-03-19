namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_Alter_QueryCode : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Queries", name: "UniqeCode", newName: "UniqueCode");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Queries", name: "UniqueCode", newName: "UniqeCode");
        }
    }
}
