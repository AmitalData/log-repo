
declare @Tenant as int
declare @QuoteId as varchar(15)
declare @QuoteStageId as varchar (15)
declare @AcceptedStageId as varchar(15)
declare @UsageCount as int

BEGIN
		DECLARE QuotesCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, StageId
		FROM Quotes
		WHERE IsClosed = 1
		OPEN QuotesCursor FETCH NEXT FROM QuotesCursor INTO @QuoteId, @Tenant, @QuoteStageId
		WHILE @@FETCH_STATUS = 0
			BEGIN

				set @AcceptedStageId = (select Id from QuoteStages where Code = 'QTAC' and Tenant = @Tenant)

				if (@QuoteStageId = @AcceptedStageId)
				begin
					set @UsageCount =  (select COUNT(*) from Shipments where Tenant = @Tenant and QuoteId = @QuoteId)
					
					update Quotes set UsageCount = @UsageCount where Id = @QuoteId
				end

			FETCH NEXT FROM QuotesCursor INTO @QuoteId,  @Tenant, @QuoteStageId
			END
		CLOSE QuotesCursor
		DEALLOCATE QuotesCursor
END