namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AmendmentStatus_LenghtName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Customs.AmendmentStatuses", "Name", c => c.String(maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("Customs.AmendmentStatuses", "Name", c => c.String(maxLength: 3, unicode: false));
        }
    }
}
