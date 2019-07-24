namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ObjectTable_CodeField_NameField_60 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ObjectTables", "CodeField", c => c.String(maxLength: 60, unicode: false));
            AlterColumn("dbo.ObjectTables", "NameField", c => c.String(maxLength: 60, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ObjectTables", "CodeField", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.ObjectTables", "NameField", c => c.String(maxLength: 15, unicode: false));

        }
    }
}
