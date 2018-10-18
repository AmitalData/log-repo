
declare @Tenant as int
declare @Id as varchar(15)
declare @oldStatusId as varchar(15)
declare @oldStatusDate as datetime

BEGIN 
		DECLARE eventsCursor CURSOR READ_ONLY
		FOR
		SELECT Id,Tenant,statusId,statusdate
		FROM Shipments
		where ShipmentLevelCode = 'C' or ShipmentLevelCode = 'D'
		OPEN eventsCursor FETCH NEXT FROM eventsCursor INTO @Id,@Tenant	,@oldStatusId,@oldStatusDate	
		WHILE @@FETCH_STATUS = 0
			BEGIN

			declare @StatusDate datetime
			declare @StatusId varchar(15)
			
		
          set @StatusId=(select top(1) id from EntityStatus where ObjectTableId =(select id from objecttables where name='Shipment') and Tenant=@Tenant 
          and id in (select entitystatusid from EventTypes where id in (select eventtypeid from TraceEvents where EntityId=@Id)) order by StatusWeight desc)

          set @StatusDate = (select top(1) EventDateTime from TraceEvents where EntityId=@Id and Tenant=@Tenant and EventTypeId in
          (select id from EventTypes where EntityStatusId = @StatusId and Tenant=@Tenant and ObjectTableId=(select id from objecttables where name='Shipment')))

print @StatusId
print @StatusDate
			
			if(@oldStatusId <> @StatusId or @oldStatusDate <> @StatusDate)
			begin
		    update ShipmentMasterDatas set StatusId=@StatusId,StatusDate=@StatusDate where Id=@Id and Tenant=@Tenant

			 update shipments set StatusId=@StatusId,StatusDate=@StatusDate where Id=@Id and Tenant=@Tenant
			 end

				FETCH NEXT FROM eventsCursor INTO @Id,@Tenant,@oldStatusId,@oldStatusDate				
			END
		CLOSE eventsCursor
		DEALLOCATE eventsCursor
END

--declare @StatusDate datetime
--declare @StatusId varchar(15)
--set @StatusId=(select top(1) id from EntityStatus where ObjectTableId =(select id from objecttables where name='Shipment') and Tenant=1 
--and id in (select entitystatusid from EventTypes where id in (select eventtypeid from TraceEvents where EntityId='1-464')) order by StatusWeight desc)

--set @StatusDate = (select top(1) EventDateTime from TraceEvents where EntityId='1-464' and Tenant=1 and EventTypeId in (select id from EventTypes where EntityStatusId = @StatusId and Tenant=1 and ObjectTableId=(select id from objecttables where name='Shipment')))

--print @StatusId
--print @StatusDate



--(select top(1) EventDateTime,(select EnglishName from EventTypes where id=EventTypeId) from TraceEvents where EntityId='1-812' and Tenant=1 and EventTypeId =(select top(1) id from EventTypes where id in (select eventtypeid from TraceEvents where EntityId='1-812') and ObjectTableId=(select id from objecttables where name='Shipment') and Tenant=1 and EntityStatusId=(select top(1) id from EntityStatus where ObjectTableId =(select id from objecttables where name='Shipment') and Tenant=1 order by StatusWeight desc)) )

--select *,(select EnglishName from EventTypes where id=EventTypeId),(select statusweight from EntityStatus where Id=(select entitystatusId from EventTypes where id=EventTypeId)),(select name from objecttables where id=ObjectTableId)
--from TraceEvents where EntityId='1-701'

--select statusid from ShipmentMasterDatas where id ='1-77'
----select top(1) id,Name,StatusWeight from EntityStatus where ObjectTableId =(select id from objecttables where name='Shipment') and Tenant=1 
----and id in (select entitystatusid from EventTypes where id in (select eventtypeid from TraceEvents where EntityId='1-77')) order by StatusWeight desc


--SELECT        Id, StatusId, StatusDate,
--                             (SELECT        Name
--                               FROM            EntityStatus
--                               WHERE        (Id = ShipmentMasterDatas.StatusId)) AS Expr1,(select shipmentlevelcode from Shipments where shipments.Id=ShipmentMasterDatas.Id)
                            
--FROM            ShipmentMasterDatas


select  top(10) shipments.id,shipments.statusid,shipments.ShipmentNumber,TraceEvents.* from Shipments left outer join TraceEvents on shipments.id=traceevents.EntityId left outer join EventTypes on TraceEvents.EventTypeId=EventTypes.Id left outer join EntityStatus on EventTypes.EntityStatusId = entityStatus.Id