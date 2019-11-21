namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class oibjectfield_fiedcode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ObjectFields", "FieldCode", c => c.String(nullable: true, maxLength: 200, unicode: false));
            Sql(@"update objectfields set FieldCode = ((select objecttables.Name from objecttables where id = ObjectTableId) + '.' + FieldName) where IsCustom = 0");
            Sql(@"update objectfields set FieldCode = ((select objecttables.Name from objecttables where id = ObjectTableId) +'.' + +CAST(Tenant as varchar) + '.' + FieldName) where IsCustom = 1");
            
            AlterColumn("dbo.ObjectFields", "FieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ObjectFields", "FieldCode");
        }
    }
}
