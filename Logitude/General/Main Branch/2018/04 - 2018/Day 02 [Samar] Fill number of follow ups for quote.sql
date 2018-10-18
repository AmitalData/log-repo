
declare @Tenant as int
declare @QuoteId as varchar(15)
declare @FollowUpsCount as int

BEGIN 
	DECLARE QuotesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant
	FROM Quotes	
	OPEN QuotesCursor FETCH NEXT FROM QuotesCursor INTO @QuoteId, @Tenant
	WHILE @@FETCH_STATUS = 0
		BEGIN

		set @FollowUpsCount = (select COUNT(*) from FollowUps where Tenant = @Tenant and QuoteId = @QuoteId)

		if(@FollowUpsCount > 0)
		begin

			update Quotes set NumberOfFollowUps = @FollowUpsCount where Id = @QuoteId

		end

			FETCH NEXT FROM QuotesCursor INTO @QuoteId, @Tenant
		END
	CLOSE QuotesCursor
	DEALLOCATE QuotesCursor
END


