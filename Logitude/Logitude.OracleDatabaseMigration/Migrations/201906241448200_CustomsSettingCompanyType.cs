namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomsSettingCompanyType : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.CustomsSettings", "CompanyType", c => c.String(nullable: false, maxLength: 1, unicode: false,defaultValue:"C"));
        }
        
        public override void Down()
        {
            DropColumn("Customs.CustomsSettings", "CompanyType");
        }
    }
}
