declare @Tenant integer
declare @ClassificationId as varchAR(15)
declare @DefaultId as varchAR(15)

BEGIN 

	DECLARE ClassificationsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant
	From TicketClassifications
	OPEN ClassificationsCursor FETCH NEXT FROM ClassificationsCursor INTO @ClassificationId, @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		set @DefaultId = (SELECT Id FROM TicketSeverities where Tenant = @Tenant and Code = 'MD')

		update TicketClassifications set DefaultSeverityId = @DefaultId where Id = @ClassificationId and Name = 'General'

	FETCH NEXT FROM ClassificationsCursor INTO @ClassificationId, @Tenant	
	END

	CLOSE ClassificationsCursor
	DEALLOCATE ClassificationsCursor
END