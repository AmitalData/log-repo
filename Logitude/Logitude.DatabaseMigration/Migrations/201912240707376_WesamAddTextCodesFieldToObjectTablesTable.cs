namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddTextCodesFieldToObjectTablesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ObjectTables", "DescriptionTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            Sql(@"update ObjectTables set DescriptionTextCodeCode = (select TextCodes.Code from TextCodes where id = ObjectTables.DescriptionTextCodeId)");
            AddColumn("dbo.ObjectTables", "NewButtonTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            Sql(@"update ObjectTables set NewButtonTextCodeCode = (select TextCodes.Code from TextCodes where id = ObjectTables.NewButtonTextCodeId)");
        }
        
        public override void Down()
        {
            DropColumn("dbo.ObjectTables", "NewButtonTextCodeCode");
            DropColumn("dbo.ObjectTables", "DescriptionTextCodeCode");
        }
    }
}
