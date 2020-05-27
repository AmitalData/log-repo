namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddObjectFieldCodeToRestrictionTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Restrictions", "ObjectFieldCode", c => c.String(nullable: true, maxLength: 200, unicode: false));
            Sql(@"update Restrictions set ObjectFieldCode =(select ObjectFields.FieldCode from ObjectFields where id=Restrictions.ObjectFieldId)");
            AlterColumn("dbo.Restrictions", "ObjectFieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Restrictions", "ObjectFieldCode");
        }
    }
}
