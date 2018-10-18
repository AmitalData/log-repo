--select  shipments.id,shipments.statusid,shipments.ShipmentNumber,TraceEvents.EntityId,EventTypes.EnglishName,EntityStatus.Name,EntityStatus.StatusWeight,EntityStatusId 
--from Shipments left outer join TraceEvents on shipments.id=traceevents.EntityId left outer join
-- EventTypes on TraceEvents.EventTypeId=EventTypes.Id left outer join 
-- EntityStatus on EventTypes.EntityStatusId = entityStatus.Id
-- where TraceEvents.ObjectTableId=(select id from ObjectTables where Name='Shipment')


 declare @Tenant as int
declare @ShipmentId as varchar(15)
declare @StatusId as varchar(15)
declare @StatusWeight as int
declare @EntityStatusId as varchar(15)
declare @LogDateTime as datetime
declare @counter as int
set @counter=0
declare @CurrentShipmentId as varchar(15)
set @CurrentShipmentId='begin'
BEGIN 
		DECLARE eventsCursor CURSOR READ_ONLY
		FOR
		select  shipments.id,shipments.statusid,Shipments.Tenant,EntityStatus.StatusWeight,EntityStatusId ,TraceEvents.LogDateTime
from Shipments left outer join TraceEvents on shipments.id=traceevents.EntityId left outer join
 EventTypes on TraceEvents.EventTypeId=EventTypes.Id left outer join 
 EntityStatus on EventTypes.EntityStatusId = entityStatus.Id
 where TraceEvents.ObjectTableId=(select id from ObjectTables where Name='Shipment') and EntityStatusId is not null order by shipments.Id,EntityStatus.StatusWeight desc
		OPEN eventsCursor FETCH NEXT FROM eventsCursor INTO @ShipmentId,@StatusId,@Tenant,@StatusWeight,@EntityStatusId,@LogDateTime
		WHILE @@FETCH_STATUS = 0
			BEGIN
			if(@counter=0)
			begin 
			print 'new transaction began'
			begin transaction tran1
			end
			set @counter=@counter+1
		if(@CurrentShipmentId <> @ShipmentId)
		begin
		set @CurrentShipmentId=@ShipmentId

		print 'masater'
		    update ShipmentMasterDatas set StatusId=@EntityStatusId,StatusDate=@LogDateTime where Id=@ShipmentId and Tenant=@Tenant
		print 'shipment'
			 update shipments set StatusId=@EntityStatusId,StatusDate=@LogDateTime where Id=@ShipmentId and Tenant=@Tenant

		end
		if(@counter=100)
		begin
		print 'committing'
		set @counter=0
		commit
		end

				FETCH NEXT FROM eventsCursor INTO @ShipmentId,@StatusId,@Tenant,@StatusWeight,@EntityStatusId,@LogDateTime	
			END
		CLOSE eventsCursor
		DEALLOCATE eventsCursor
END
print 'final commit'
--commit


--	select  shipments.id,shipments.statusid,Shipments.Tenant,EntityStatus.StatusWeight,EntityStatusId ,TraceEvents.LogDateTime
--from Shipments left outer join TraceEvents on shipments.id=traceevents.EntityId left outer join
-- EventTypes on TraceEvents.EventTypeId=EventTypes.Id left outer join 
-- EntityStatus on EventTypes.EntityStatusId = entityStatus.Id
-- where TraceEvents.ObjectTableId=(select id from ObjectTables where Name='Shipment') order by shipments.Id,EntityStatus.StatusWeight desc