namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddSearchFieldsToCustomerFieldsUpdateSettingTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerFieldsUpdateSettings", "SearchFields", c => c.String(maxLength: 1000));
            Sql(@"update CustomerFieldsUpdateSettings set SearchFields = ObjectFieldCode + ',' + UpdateDirection");
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerFieldsUpdateSettings", "SearchFields");
        }
    }
}
