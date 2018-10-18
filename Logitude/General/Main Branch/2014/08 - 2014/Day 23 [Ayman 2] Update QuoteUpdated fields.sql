

alter table Quotes alter column UpdateDate DateTime null
go

update Quotes set UpdateDate = OpenDate
go

update Quotes set UpdatedByUserId = CreatedByUserId
go


declare @TraceEventId varchar(40)
declare @QuoteId varchar(15)
DECLARE @UserId varchar(15)
declare @UpdateDate datetime
declare @Tenant as int

DECLARE QuotesCursor CURSOR READ_ONLY
	FOR	
	SELECT Id, Tenant
	FROM Quotes
	OPEN QuotesCursor FETCH NEXT FROM QuotesCursor INTO @QuoteId, @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @UserId = null
		set @UpdateDate = null
		set @TraceEventId = null

		set @TraceEventId = 
			(select TOP 1 Id from TraceEvents
			 where EntityId = @QuoteId
			 and Tenant = @Tenant
			 and EventTypeId = (select Id from EventTypes where Tenant = @Tenant and Code = 'UPQT' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
			 ORDER BY LogDateTime DESC
			 )			 

		if (@TraceEventId is not null)
		begin

			set @UserId = (select UserId from TraceEvents where Id = @TraceEventId)
			set @UpdateDate = (select LogDateTime from TraceEvents where Id = @TraceEventId)


			if (@UserId is not null)
			update Quotes set UpdatedByUserId = @UserId where Id = @QuoteId

			if (@UpdateDate is not null)
			update Quotes set UpdateDate = @UpdateDate where Id = @QuoteId
		end


	FETCH NEXT FROM QuotesCursor INTO @QuoteId, @Tenant	
	END
CLOSE QuotesCursor
DEALLOCATE QuotesCursor