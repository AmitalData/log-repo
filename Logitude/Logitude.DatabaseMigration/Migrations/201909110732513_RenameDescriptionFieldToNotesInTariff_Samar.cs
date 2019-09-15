namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class RenameDescriptionFieldToNotesInTariff_Samar : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Tariffs", "Description", "Notes");
            Sql(@"delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'Description' and ObjectTableId = (select Id from ObjectTables where Name = 'Tariff'))
                 delete from ObjectFields where FieldName = 'Description' and ObjectTableId = (select Id from ObjectTables where Name = 'Tariff')
                 delete from TextCodes where Code like '%Description%' and ObjectTableId = (select Id from ObjectTables where Name = 'Tariff')");
        }

        public override void Down()
        {
            RenameColumn("dbo.Tariffs", "Notes", "Description");
        }
    }
}