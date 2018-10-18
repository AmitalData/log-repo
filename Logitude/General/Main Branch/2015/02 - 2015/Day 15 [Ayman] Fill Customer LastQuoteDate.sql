
declare @Tenant as int
declare @CustomerId as varchar(15)
declare @QuoteLastDate as datetime

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT CustomerId, Tenant, max(OpenDate)
	FROM Quotes
	where CustomerId is not null
	group by CustomerId, Tenant
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @CustomerId, @Tenant, @QuoteLastDate
	WHILE @@FETCH_STATUS = 0
	BEGIN

		update Customers
		set LastQuoteDate = @QuoteLastDate
		where Id = @CustomerId and Tenant = @Tenant
		
	FETCH NEXT FROM DataCursor INTO @CustomerId, @Tenant, @QuoteLastDate	
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor
END
