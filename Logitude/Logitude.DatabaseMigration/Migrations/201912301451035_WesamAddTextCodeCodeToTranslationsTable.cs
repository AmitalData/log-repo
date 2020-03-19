namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddTextCodeCodeToTranslationsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Translations", "TextCodeCode", c => c.String(nullable: true, maxLength: 100, unicode: false));
            Sql(@"update Translations set TextCodeCode = (select TextCodes.Code from TextCodes where id = Translations.TextCodeId)");
            AlterColumn("dbo.Translations", "TextCodeCode", c => c.String(nullable: false, maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Translations", "TextCodeCode");
        }
    }
}
