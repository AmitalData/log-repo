namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAmendmentDontDisplayInList : DbMigration
    {
        public override void Up()
        {
             AddColumn("Customs.Declarations", "AmendmentDontDisplayInList", c => c.Boolean(nullable: false));
         }
        
        public override void Down()
        {
             DropColumn("Customs.Declarations", "AmendmentDontDisplayInList");
         }
    }
}
