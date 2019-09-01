namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteSendToCustomsFeaures_Samar : DbMigration
    {
        public override void Up()
        {
            Sql(@"delete from PackageFeatures where FeatureId = (select Id from Features where Code = 'SendToCustoms' and ObjectTableId = (select Id from ObjectTables where Name = 'Shipment'))
                delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'SendToCustoms' and ObjectTableId = (select Id from ObjectTables where Name = 'Shipment'))
                delete from Features where Code = 'SendToCustoms' and ObjectTableId = (select Id from ObjectTables where Name = 'Shipment')");
        }
        
        public override void Down()
        {
            
        }
    }
}
