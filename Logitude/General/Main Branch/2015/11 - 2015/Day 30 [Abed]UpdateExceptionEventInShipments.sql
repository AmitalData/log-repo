
declare @ShipmentId as varchar(15)
declare @HasException as bit
declare @Note as varchar(100)
declare @EventDateTime as datetime



	DECLARE TraceEventsCursor CURSOR READ_ONLY
	FOR
	SELECT EntityId , Notes, EventDateTime
	From TraceEvents
	INNER JOIN EventTypes ON TraceEvents.EventTypeId= EventTypes.Id
	WHERE EventTypes.Code = 'EXCE'
	OPEN TraceEventsCursor FETCH NEXT FROM TraceEventsCursor INTO @ShipmentId , @Note ,@EventDateTime
	WHILE @@FETCH_STATUS = 0
	BEGIN

	
	 set @HasException = (select HasException from Shipments where Id = @ShipmentId)
	
	if(@HasException = 0)
	   begin
        update Shipments set HasException = 1 , ExceptionDescription = @Note ,ExceptionDate =@EventDateTime    where Id = @ShipmentId 
		end

	FETCH NEXT FROM TraceEventsCursor INTO @ShipmentId , @Note ,@EventDateTime

	End
	CLOSE TraceEventsCursor
	DEALLOCATE TraceEventsCursor
