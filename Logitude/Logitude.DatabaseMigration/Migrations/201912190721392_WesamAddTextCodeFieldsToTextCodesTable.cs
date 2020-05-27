namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddTextCodeFieldsToTextCodesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ObjectFields", "FullNameTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            Sql(@"update ObjectFields set FullNameTextCodeCode = (select TextCodes.Code from TextCodes where id = ObjectFields.FullNameTextCodeId)");
            AddColumn("dbo.ObjectFields", "HelpTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            Sql(@"update ObjectFields set HelpTextCodeCode = (select TextCodes.Code from TextCodes where id = ObjectFields.HelpTextCodeId)");
            AddColumn("dbo.ObjectFields", "ListTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            Sql(@"update ObjectFields set ListTextCodeCode = (select TextCodes.Code from TextCodes where id = ObjectFields.ListTextCodeId)");
            AddColumn("dbo.ObjectFields", "ShortNameTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            Sql(@"update ObjectFields set ShortNameTextCodeCode = (select TextCodes.Code from TextCodes where id = ObjectFields.ShortNameTextCodeId)");
        }

        public override void Down()
        {
            DropColumn("dbo.ObjectFields", "ShortNameTextCodeCode");
            DropColumn("dbo.ObjectFields", "ListTextCodeCode");
            DropColumn("dbo.ObjectFields", "HelpTextCodeCode");
            DropColumn("dbo.ObjectFields", "FullNameTextCodeCode");
        }
    }
}
