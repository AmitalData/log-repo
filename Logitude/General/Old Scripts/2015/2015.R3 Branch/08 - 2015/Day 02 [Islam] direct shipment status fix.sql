--Select Id,tenant,shipmentnumber,statusid,StatusDate,MasterShipmentDataId,ShipmentLevelCode from Shipments where ShipmentNumber = 'SHE200138'
--select StatusDate,StatusId from ShipmentMasterDatas where id = '1-1230'


declare @Tenant as int
declare @Id as varchar(15)
declare @currentStatusId as varchar(15)
declare @currentStatusDate as datetime
 declare @mastershipmentdataId as varchar(15)
BEGIN
              DECLARE eventsCursor CURSOR READ_ONLY
              FOR
              SELECT Id,Tenant,statusId,statusdate,MasterShipmentDataId
              FROM Shipments
              where ShipmentLevelCode = 'D'
              OPEN eventsCursor FETCH NEXT FROM eventsCursor INTO @Id,@Tenant       ,@currentStatusId,@currentStatusDate,@mastershipmentdataId    
              WHILE @@FETCH_STATUS = 0
                     BEGIN
 
                     declare @StatusDate datetime
                     declare @StatusId varchar(15)
                    
             
          set @StatusId=(select top(1) StatusId from ShipmentMasterDatas where id=@mastershipmentdataId and Tenant=@Tenant )
        
 
          set @StatusDate =(select top(1) StatusDate from ShipmentMasterDatas where Id=@mastershipmentdataId and Tenant=@Tenant )
 
--print @StatusId
--print @StatusDate
                    
                     if(@StatusId is null or @StatusDate is null)
                     begin
                      update ShipmentMasterDatas set StatusId=@currentStatusId,StatusDate=@currentStatusDate where Id=@mastershipmentdataId and Tenant=@Tenant
 
                       print @Id
                     end
 
                           FETCH NEXT FROM eventsCursor INTO @Id,@Tenant,@currentStatusId,@currentStatusDate ,@mastershipmentdataId                        
                     END
              CLOSE eventsCursor
              DEALLOCATE eventsCursor
END


--select id,tenant,statusid,statusdate from ShipmentMasterDatas where StatusDate  is null and id in (select MasterShipmentDataId from shipments where ShipmentLevelCode = 'D' and StatusDate is not null and StatusId is not null)