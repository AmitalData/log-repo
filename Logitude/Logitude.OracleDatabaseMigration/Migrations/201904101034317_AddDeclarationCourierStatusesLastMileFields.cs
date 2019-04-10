namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeclarationCourierStatusesLastMileFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.DeclarationCourierStatuses", "LastMileStatusCode", c => c.String(maxLength: 30, unicode: false));
            AddColumn("Customs.DeclarationCourierStatuses", "LastMileStatusDate", c => c.DateTime(precision: 7));
            AddColumn("Customs.DeclarationCourierStatuses", "LastMileStatusRemarks", c => c.String(maxLength: 2000));
        }
        
        public override void Down()
        {
            DropColumn("Customs.DeclarationCourierStatuses", "LastMileStatusRemarks");
            DropColumn("Customs.DeclarationCourierStatuses", "LastMileStatusDate");
            DropColumn("Customs.DeclarationCourierStatuses", "LastMileStatusCode");
        }
    }
}
