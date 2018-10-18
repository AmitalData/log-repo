
declare @Tenant as int
declare @TableId as varchar(15)
declare @EventId varchar(15)
declare @EventDate datetime 
declare @ShipmentId varchar(15)
declare @EventTypeId varchar(15)

declare @EventLocation nvarchar(40)
declare @EventNotes nvarchar(4000)
declare @TableIdShipment as varchar(15)
declare @TableIdMaster as varchar(15)
set @TableIdShipment = (select Id from ObjectTables where Name = 'Shipment')
set @TableIdMaster = (select Id from ObjectTables where Name = 'Master')

	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT TraceEvents.EntityId, TraceEvents.ObjectTableId, TraceEvents.Tenant, MAX(TraceEvents.EventDateTime)
	FROM TraceEvents join EventTypes On (TraceEvents.EventTypeId  = EventTypes.Id AND TraceEvents.Tenant = EventTypes.Tenant)		
	where EventTypes.IsCustomerView = 1	AND EventTypes.ObjectTableId in (@TableIdShipment, @TableIdMaster)
	group by TraceEvents.EntityId, TraceEvents.ObjectTableId, TraceEvents.Tenant
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @ShipmentId ,@TableId, @Tenant,@EventDate     
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
		select top 1
		@EventNotes =  Notes , @EventLocation = Location, @EventTypeId = EventTypeId
		from TraceEvents
		where EntityId = @ShipmentId
		and Tenant = @Tenant 
		and ObjectTableId = @TableId 
		and EventDateTime = @EventDate
	
		update Shipments 
		set LastSharedEventId = @EventTypeId,
		LastSharedEventDate = @EventDate,
		LastSharedEventLocation = @EventLocation,
		LastSharedEventNotes = @EventNotes
		where Id = @ShipmentId and Tenant= @Tenant

    FETCH NEXT FROM DataCursor INTO @ShipmentId,@TableId, @Tenant,@EventDate
    END
	CLOSE DataCursor
	DEALLOCATE DataCursor