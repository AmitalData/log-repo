namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeclarationCourierStatusesCourierPendingReasonListField : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.DeclarationCourierStatuses", "CourierPendingReasonList", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("Customs.DeclarationCourierStatuses", "CourierPendingReasonList");
        }
    }
}
