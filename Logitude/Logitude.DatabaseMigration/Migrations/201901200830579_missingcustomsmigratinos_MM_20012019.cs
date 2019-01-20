namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missingcustomsmigratinos_MM_20012019 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.Declarations", "DepositionStatusCode", c => c.String(maxLength: 1, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("Customs.Declarations", "DepositionStatusCode");
        }
    }
}
