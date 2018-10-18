

declare @TenantNumber as int
declare @Id as varchar(15)
declare @EndDate as datetime
declare @Amount as float
declare @Remaining as float
declare @IsCancelled as bit
declare @Notes as nvarchar(250)
declare @MySearchFields as nvarchar(1000)

BEGIN
		DECLARE DataCursor CURSOR READ_ONLY
		FOR
		SELECT Id, TenantNumber, IsCancelled, Remaining, EndDate, Amount, Notes
		FROM AWBMessagingStocks
		OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Id, @TenantNumber, @IsCancelled, @Remaining, @EndDate, @Amount, @Notes
		WHILE @@FETCH_STATUS = 0
			BEGIN

			set @MySearchFields = 'New'

				if (@IsCancelled = 1)
				begin
					set @MySearchFields = 'Cancelled'
				end		

				else if (@Remaining = 0)
				begin
					set @MySearchFields = 'Used'
				end	

				else if (@EndDate <= CONVERT(date, getdate()))
				begin
					set @MySearchFields = 'Expired'
				end	

				else if (@Amount > @Remaining)
				begin
					set @MySearchFields = 'Active'
				end	

				if (@Notes is not null)
				begin
					if (@MySearchFields = '') set @MySearchFields = @Notes
					else set @MySearchFields = @MySearchFields + ',' + @Notes	
				end

			update AWBMessagingStocks set SearchFields = @MySearchFields where Id = @Id AND TenantNumber = @TenantNumber

			FETCH NEXT FROM DataCursor INTO @Id, @TenantNumber, @IsCancelled, @Remaining, @EndDate, @Amount, @Notes
			END
		CLOSE DataCursor
		DEALLOCATE DataCursor
END

