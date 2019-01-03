
declare @Count integer
declare @Tenant integer
declare @UserId varchar(15)
declare @EntityId varchar(15)
declare @FirstDateTime date
declare @EntityApproveDate date

BEGIN 
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	select TraceEvents.Tenant, TraceEvents.EntityId, min(TraceEvents.LogDateTime), count(*)
	from TraceEvents join EventTypes
	on
	TraceEvents.EventTypeId = EventTypes.Id
	AND TraceEvents.Tenant = EventTypes.Tenant

	where
	TraceEvents.ObjectTableId = (select Id from ObjectTables where Name = 'APPayment')
	AND EventTypes.Code = 'APPA'
	group by TraceEvents.Tenant, TraceEvents.EntityId
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Tenant,@EntityId,@FirstDateTime,@Count
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		set @EntityApproveDate = (select ApprovedDateTime from APPayments where Id = @EntityId)

		if (@EntityApproveDate is null)
		begin
				set @UserId = (
								select top 1 TraceEvents.UserId from TraceEvents join EventTypes
								on TraceEvents.EventTypeId = EventTypes.Id
								where 
								TraceEvents.ObjectTableId = (select Id from ObjectTables where Name = 'APPayment')
								AND TraceEvents.Tenant = @Tenant
								AND TraceEvents.EntityId = @EntityId
								AND cast(TraceEvents.LogDateTime as date) = @FirstDateTime
								AND EventTypes.Code = 'APPA'
							  )

				update APPayments
				set
				ApprovedDateTime = @FirstDateTime,
				FirstApproveDate = @FirstDateTime,
				ApprovedByUserId = @UserId
				where Id = @EntityId
		end


		else if (@Count = 1)
		begin			
			update APPayments set FirstApproveDate = ApprovedDateTime where Id = @EntityId
		end

		else
		begin
			update APPayments set FirstApproveDate = @FirstDateTime where Id = @EntityId
		end

	FETCH NEXT FROM DataCursor INTO @Tenant,@EntityId,@FirstDateTime,@Count	
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor

END