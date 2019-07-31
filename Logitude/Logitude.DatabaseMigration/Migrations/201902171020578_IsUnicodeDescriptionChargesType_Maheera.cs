namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsUnicodeDescriptionChargesType_Maheera : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ChargesTypes", "Description", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ChargesTypes", "Description", c => c.String(maxLength: 250, unicode: false));
        }
    }
}
