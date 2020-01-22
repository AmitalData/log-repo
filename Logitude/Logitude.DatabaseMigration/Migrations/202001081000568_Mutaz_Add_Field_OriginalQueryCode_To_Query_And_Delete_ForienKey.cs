namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_Add_Field_OriginalQueryCode_To_Query_And_Delete_ForienKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Queries", "OriginalQueryCode", c => c.String( maxLength: 30, unicode: false));
            Sql(@"update Queries set OriginalQueryCode =  Queries.Code");
            Sql("ALTER TABLE Queries DROP CONSTRAINT  FK_QueryQuery");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Queries", "OriginalQueryCode");
        }
    }
}
