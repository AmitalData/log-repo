namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteCutOffDateFieldFromShipment_Maheera : DbMigration
    {
        public override void Up()
        {
            Sql("declare @Tenant as int declare @MasterShipmentDataId as varchar(15) declare @EntityId as varchar(15) declare @CutoffDate as datetime BEGIN DECLARE DataCursor CURSOR READ_ONLY FOR SELECT Id, Tenant, MasterShipmentDataId, CutoffDate FROM Shipments where MasterShipmentDataId is not null and CutoffDate is not null OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @MasterShipmentDataId, @CutoffDate WHILE @@FETCH_STATUS = 0 BEGIN BEGIN update ShipmentMasterDatas set CutoffDate = @CutoffDate where Tenant = @Tenant and Id = @MasterShipmentDataId END FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @MasterShipmentDataId, @CutoffDate END CLOSE DataCursor DEALLOCATE DataCursor END");
            DropColumn("dbo.Shipments", "CutoffDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Shipments", "CutoffDate", c => c.DateTime());
        }
    }
}
