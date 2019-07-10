namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missing1206 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.Clients", "NationalIdentificationNumber", c => c.String(maxLength: 25, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("Customs.Clients", "NationalIdentificationNumber");
        }
    }
}
