
DECLARE @QuoteId AS VARCHAR(15)
DECLARE @Tenant AS INT

BEGIN;

	DECLARE QuoteIdCursor CURSOR READ_ONLY
	FOR	
	SELECT Id, Tenant
	FROM Quotes	 
	OPEN QuoteIdCursor FETCH NEXT FROM QuoteIdCursor INTO @QuoteId, @Tenant
	WHILE @@FETCH_STATUS = 0
		BEGIN

		DECLARE @ShipmentCount as int
		DECLARE @Date as DateTime

		set @ShipmentCount = 0
		set @Date = null

		if exists (select * from Shipments where QuoteId = @QuoteId and Tenant = @Tenant)
		begin
			set @ShipmentCount = (select COUNT(*) from shipments where QuoteId = @QuoteId and Tenant = @Tenant)
			set @Date = (select MAX(CreateDateTime) from shipments where QuoteId = @QuoteId and Tenant = @Tenant)
		end

		update Quotes
		set UsageCount = @ShipmentCount, LastUsageDate = @Date
		where Id = @QuoteId and Tenant = @Tenant

	FETCH NEXT FROM QuoteIdCursor INTO @QuoteId,@Tenant
	END
	CLOSE QuoteIdCursor
	DEALLOCATE QuoteIdCursor	
END
go

