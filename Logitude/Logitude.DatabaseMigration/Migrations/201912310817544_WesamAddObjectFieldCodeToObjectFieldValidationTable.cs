namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddObjectFieldCodeToObjectFieldValidationTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ObjectFieldValidations", "ObjectFieldCode", c => c.String(nullable: true, maxLength: 200, unicode: false));
            Sql(@"update ObjectFieldValidations set ObjectFieldCode = (select ObjectFields.FieldCode from ObjectFields where id = ObjectFieldValidations.ObjectFieldId)");
            AlterColumn("dbo.ObjectFieldValidations", "ObjectFieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ObjectFieldValidations", "ObjectFieldCode");
        }
    }
}
