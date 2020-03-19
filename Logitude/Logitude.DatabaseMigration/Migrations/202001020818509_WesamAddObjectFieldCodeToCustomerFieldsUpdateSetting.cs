namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddObjectFieldCodeToCustomerFieldsUpdateSetting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerFieldsUpdateSettings", "ObjectFieldCode", c => c.String(maxLength: 200, unicode: false));
            Sql(@"update CustomerFieldsUpdateSettings set ObjectFieldCode = (select ObjectFields.FieldCode from ObjectFields where id = CustomerFieldsUpdateSettings.ObjectFieldId)");
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerFieldsUpdateSettings", "ObjectFieldCode");
        }
    }
}
