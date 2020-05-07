namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddOriginalObjectFieldCodeToDWObjectField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DWObjectFields", "OriginalObjectFieldCode", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DWObjectFields", "OriginalObjectFieldCode");
        }
    }
}
