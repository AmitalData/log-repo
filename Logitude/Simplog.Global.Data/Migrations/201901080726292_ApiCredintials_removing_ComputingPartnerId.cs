namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApiCredintials_removing_ComputingPartnerId : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ApiCredintials", "ComputingPartnerId");
            Sql(@"
delete from ObjectFields where FieldName = 'ComputingPartnerId' and ObjectTableId in (select Id from ObjectTables where Name = 'ApiCredintials')
delete from TextCodes where Code like '%.Computing%' and ObjectTableId in (select Id from ObjectTables where Name = 'ApiCredintials')");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApiCredintials", "ComputingPartnerId", c => c.String(maxLength: 15, unicode: false));
        }
    }
}
