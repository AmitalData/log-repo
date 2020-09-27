namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missing23042020 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("Customs.CustomsRequiredFields", "ObjectfieldCode", c => c.String(maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("Customs.CustomsRequiredFields", "ObjectfieldCode");
        }
    }
}
