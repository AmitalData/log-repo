namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_Add_Field_HeaderScreenCode_To_ObjectTable_And_Remove_ForiegnKey : DbMigration
    {
        public override void Up()
        {

            AddColumn("dbo.ObjectTables", "HeaderScreenCode", c => c.String(maxLength: 100, unicode: false));
            Sql(@"update ObjectTables set HeaderScreenCode =(select Screens.Code from Screens where Id=ObjectTables.HeaderScreenId)");
            Sql("ALTER TABLE ObjectTables DROP CONSTRAINT FK_ScreenObjectTable");
        }
        
        public override void Down()
        {
            DropColumn("dbo.ObjectTables", "HeaderScreenCode");
        }
    }
}
