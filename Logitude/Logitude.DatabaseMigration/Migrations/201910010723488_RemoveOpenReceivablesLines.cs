namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveOpenReceivablesLines : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Shipments", "OpenReceivablesLines");

            Sql("delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'OpenReceivablesLines')");
            Sql("delete from ObjectFields where FieldName = 'OpenReceivablesLines'");
            Sql("delete from TextCodes where Code = 'Shipment.CH.OpenReceivablesLinesListLable'");
            Sql("delete from TextCodes where Code = 'Shipment.F.OpenReceivablesLines'");
            Sql("delete from TextCodes where Code = 'Shipment.OpenReceivablesLinesHelpText'");        
        }

        public override void Down()
        {
            AddColumn("dbo.Shipments", "OpenReceivablesLines", c => c.Int(nullable: false));
        }
    }
}
