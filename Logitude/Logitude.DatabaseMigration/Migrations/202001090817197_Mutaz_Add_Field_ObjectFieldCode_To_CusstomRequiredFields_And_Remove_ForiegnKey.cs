namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_Add_Field_ObjectFieldCode_To_CusstomRequiredFields_And_Remove_ForiegnKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.CustomsRequiredFields", "ObjectfieldCode", c => c.String(nullable: true, maxLength: 100, unicode: false));
            Sql(@"update Customs.CustomsRequiredFields set ObjectfieldCode =(select ObjectFields.FieldCode from ObjectFields where id=CustomsRequiredFields.ObjectFieldId)");
            AlterColumn("Customs.CustomsRequiredFields", "ObjectfieldCode", c => c.String(nullable: false, maxLength: 100, unicode: false));
            Sql("ALTER TABLE Customs.CustomsRequiredFields DROP CONSTRAINT[FK_Customs.CustomsRequiredFields_dbo.ObjectFields_ObjectfieldId]");

        }

        public override void Down()
        {
            DropColumn("Customs.CustomsRequiredFields", "ObjectfieldCode");
        }
    }
}
