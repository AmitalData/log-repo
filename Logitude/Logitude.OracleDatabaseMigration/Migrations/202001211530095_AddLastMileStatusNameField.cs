namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLastMileStatusNameField : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.DeclarationCourierStatuses", "LastMileStatusName", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("Customs.DeclarationCourierStatuses", "LastMileStatusCode", c => c.String(maxLength: 3, unicode: false));
 
        }
        
        public override void Down()
        {
             AlterColumn("Customs.DeclarationCourierStatuses", "LastMileStatusCode", c => c.String(maxLength: 30, unicode: false));
            DropColumn("Customs.DeclarationCourierStatuses", "LastMileStatusName");
        }
    }
}
