namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropSearchFieldColumnFromCustomsTransferLine_Samar : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CustomsTransferLines", "SearchFields");

            Sql(@"delete from ObjectFields where FieldName = 'searchFields' and ObjectTableId = (select Id from ObjectTables where Name = 'CustomsTransferLine')
                  delete from TextCodes where code like '%searchFields%' and ObjectTableId = (select Id from ObjectTables where Name = 'CustomsTransferLine')");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomsTransferLines", "SearchFields", c => c.String(maxLength: 1000));
        }
    }
}
