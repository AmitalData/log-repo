begin
declare @ShipmentId as varchar(15)
declare @Id as varchar(15)
declare @DirectionId as varchar(15)
declare @TransportModeId as varchar(1)
declare @FromPortId as varchar(15)
declare @ToPortId as varchar(15)
declare @Tenant as int

declare @MasterDataId as varchar(15)
declare @MasterShipmentDataId as varchar(15)
declare @Transshipment3ToPortId as varchar(15)
declare @Transshipment2ToPortId as varchar(15)
declare @Transshipment1ToPortId as varchar(15)
declare @MainCarriageToPortId as varchar(15)


       DECLARE WarehouseReleaseCursor CURSOR READ_ONLY
       FOR
       SELECT Id,ShipmentId,Tenant
       From WarehouseReleases
       where FromPortId is null
       OPEN WarehouseReleaseCursor FETCH NEXT FROM WarehouseReleaseCursor INTO @Id, @ShipmentId, @Tenant
       WHILE @@FETCH_STATUS = 0
       BEGIN

       
              set @DirectionId = (select DirectionId from Shipments where  Id = @ShipmentId and tenant =@Tenant )
              set @TransportModeId = (select TransportModeId from Shipments where  Id = @ShipmentId and tenant =@Tenant )
              set @FromPortId = (select FromPortId from Shipments where  Id = @ShipmentId and tenant =@Tenant )
              


       set @MasterShipmentDataId = (select MasterShipmentDataId from Shipments where  Id = @ShipmentId and tenant =@Tenant )
          set @MasterDataId = (select Id from ShipmentMasterDatas where  Id = @MasterShipmentDataId and tenant =@Tenant )
              
          if(@DirectionId != 'D' or @TransportModeId != 'I')
                           BEGIN

                           --Master
                              if(@MasterDataId is not null)
                           
                             BEGIN


              set @Transshipment3ToPortId = (select Transshipment3ToPortId from ShipmentMasterDatas where  Id = @MasterShipmentDataId and tenant =@Tenant )
              set @Transshipment2ToPortId = (select Transshipment2ToPortId from ShipmentMasterDatas where  Id = @MasterShipmentDataId and tenant =@Tenant )
              set @Transshipment1ToPortId = (select Transshipment1ToPortId from ShipmentMasterDatas where  Id = @MasterShipmentDataId and tenant =@Tenant )
              set @MainCarriageToPortId = (select MainCarriageToPortId from ShipmentMasterDatas where  Id = @MasterShipmentDataId and tenant =@Tenant )
       

          --From Port

                     --To Port
                     if(@MainCarriageToPortId is not null)BEGIN set @ToPortId = @MainCarriageToPortId;END
                     if(@Transshipment1ToPortId is not null)BEGIN set @ToPortId = @Transshipment1ToPortId;END
                  if(@Transshipment2ToPortId is not null)BEGIN set @ToPortId = @Transshipment2ToPortId;END
                  if(@Transshipment3ToPortId is not null)BEGIN set @ToPortId = @Transshipment3ToPortId;END
       
       
       
                             End


                             --House
                             else

                             BEGIN
                             set @ToPortId = (select ToPortId from Shipments where  Id = @ShipmentId and tenant =@Tenant )
                           
                             End

                    Update WarehouseReleases set FromPortId = @FromPortId , ToPortId = @ToPortId where Id = @Id and tenant =@Tenant

                      End

         Update  WarehouseReleases set DirectionId = @DirectionId where Id = @Id and tenant =@Tenant


       FETCH NEXT FROM WarehouseReleaseCursor INTO   @Id, @ShipmentId, @Tenant
       END
       CLOSE WarehouseReleaseCursor
       DEALLOCATE WarehouseReleaseCursor
END

