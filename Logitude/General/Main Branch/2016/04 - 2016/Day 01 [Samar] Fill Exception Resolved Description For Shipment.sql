
declare @ShipmentId as varchar(15)
declare @Note as varchar(100)

	DECLARE TraceEventsCursor CURSOR READ_ONLY
	FOR
	SELECT EntityId , Notes
	From TraceEvents
	INNER JOIN EventTypes ON TraceEvents.EventTypeId = EventTypes.Id
	WHERE EventTypes.Code = 'EXRE'
	OPEN TraceEventsCursor FETCH NEXT FROM TraceEventsCursor INTO @ShipmentId , @Note
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
        update Shipments set ExceptionResolvedDescription = @Note where Id = @ShipmentId 
			
	FETCH NEXT FROM TraceEventsCursor INTO @ShipmentId , @Note

	End
	CLOSE TraceEventsCursor
	DEALLOCATE TraceEventsCursor