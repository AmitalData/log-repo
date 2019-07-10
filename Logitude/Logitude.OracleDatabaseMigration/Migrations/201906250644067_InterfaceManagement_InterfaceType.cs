namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InterfaceManagement_InterfaceType : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.InterfaceManagements", "InterfaceType", c => c.String(maxLength: 1, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("Customs.InterfaceManagements", "InterfaceType");
        }
    }
}
