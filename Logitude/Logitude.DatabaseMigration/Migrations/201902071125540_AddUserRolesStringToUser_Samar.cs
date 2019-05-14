namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserRolesStringToUser_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "UserRoles", c => c.String(maxLength: 400, unicode: false));           
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "UserRoles");            
        }
    }
}
