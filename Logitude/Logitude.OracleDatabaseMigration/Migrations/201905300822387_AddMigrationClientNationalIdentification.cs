namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigrationClientNationalIdentification : DbMigration
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
