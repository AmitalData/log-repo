declare @Tenant as int
declare @EntityId as varchar(15)
declare @OpenDate as datetime
declare @MyRequestDate as datetime

BEGIN
		DECLARE QuotesCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, OpenDate
		FROM Quotes
		OPEN QuotesCursor FETCH NEXT FROM QuotesCursor INTO @EntityId, @Tenant, @OpenDate
		WHILE @@FETCH_STATUS = 0
		BEGIN

		set @MyRequestDate = (SELECT CreateDate FROM Tickets where Tenant = @Tenant and QuoteId = @EntityId)
		if(@MyRequestDate is null)
		begin
			set @MyRequestDate = @OpenDate
		end
		update Quotes set RequestDate = @MyRequestDate where Id = @EntityId AND Tenant = @Tenant

		FETCH NEXT FROM QuotesCursor INTO @EntityId, @Tenant, @OpenDate
		END				
		CLOSE QuotesCursor
		DEALLOCATE QuotesCursor
END