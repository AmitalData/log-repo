
declare @BookingId as varchar(15)
declare @StatId as varchar(15)
declare @Master as varchar(16)
declare @FFRStatusCode as varchar(3)
declare @FFRStatusName as varchar(60)

	DECLARE BookingsCursor CURSOR READ_ONLY
	FOR
	SELECT BookingId, Id
	From AirlineStatistics
	where MessageType = 'FFR'
	OPEN BookingsCursor FETCH NEXT FROM BookingsCursor INTO @BookingId, @StatId
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
		set @FFRStatusCode = ( select FFRStatusCode from Bookings where Id = @BookingId)

		set @FFRStatusName = (select Name from FFRStatus where Code = @FFRStatusCode)
		
		print @StatId + '  ' + @bookingid + '  ' + @FFRStatusCode + '   ' + @FFRStatusName
        update AirlineStatistics set MessagingStatus = @FFRStatusName where BookingId = @BookingId and Id = @StatId
			
	FETCH NEXT FROM BookingsCursor INTO @BookingId, @StatId

	End
	CLOSE BookingsCursor
	DEALLOCATE BookingsCursor