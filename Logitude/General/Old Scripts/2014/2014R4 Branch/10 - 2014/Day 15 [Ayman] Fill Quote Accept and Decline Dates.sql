

-- EventTypes
-- UPQT: Quote Updated
-- QTCP: Quote Accepted
-- QTDL: Quote Declined
-- RQTD: Return To Draft


declare @Tenant as int
declare @EntityId as varchar(15)
declare @QuoteStageId as varchar(15)
declare @QuoteStageCode as varchar(4)
declare @AcceptedDate as datetime
declare @DeclinedDate as datetime
declare @QuoteUpdateDate as datetime
declare @EventTypeId as varchar(15)
declare @IsClosed as bit

BEGIN
		DECLARE DataCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, StageId, AcceptedDate, DeclinedDate, IsClosed, UpdateDate
		FROM Quotes
		where StageId is not null
		OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @QuoteStageId, @AcceptedDate, @DeclinedDate, @IsClosed, @QuoteUpdateDate
		WHILE @@FETCH_STATUS = 0
			BEGIN

			set @QuoteStageCode = (select Code from QuoteStages where Id = @QuoteStageId and Tenant = @Tenant)
			if (@QuoteStageCode is not null)
			begin
				
				if (@QuoteStageCode = 'QTAC' OR @QuoteStageCode = 'QTDC')
				BEGIN

					if (@IsClosed = 0)
					begin
						set @IsClosed = 1
					end

					if (@QuoteStageCode != 'QTAC')
					begin
						set @AcceptedDate = null
					end

					if (@QuoteStageCode != 'QTDC')
					begin
						set @DeclinedDate = null
					end

					-- Stage Accepted
					if (@QuoteStageCode = 'QTAC')
					begin
						
						set @AcceptedDate = @QuoteUpdateDate

                        set @EventTypeId = (select Id from EventTypes where Code = 'QTCP' and Tenant = @Tenant and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')) 
						if (@EventTypeId is not null)
						begin
							if exists (select * from TraceEvents where Tenant = @Tenant and EntityId = @EntityId and EventTypeId = @EventTypeId and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))							
							set @AcceptedDate = (select Max(EventDateTime) from TraceEvents where Tenant = @Tenant and EntityId = @EntityId and EventTypeId = @EventTypeId and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))							
						end
					end

					-- Stage Declined
					else if (@QuoteStageCode = 'QTDC')
					begin

						set @DeclinedDate = @QuoteUpdateDate

                        set @EventTypeId = (select Id from EventTypes where Code = 'QTDL' and Tenant = @Tenant and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')) 
						if (@EventTypeId is not null)
						begin
							if exists (select * from TraceEvents where Tenant = @Tenant and EntityId = @EntityId and EventTypeId = @EventTypeId and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
							set @DeclinedDate = (select Max(EventDateTime) from TraceEvents where Tenant = @Tenant and EntityId = @EntityId and EventTypeId = @EventTypeId and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
						end

					end

					update Quotes
					set IsClosed = @IsClosed,
					AcceptedDate = @AcceptedDate,
					DeclinedDate = @DeclinedDate
					where Id = @EntityId AND Tenant = @Tenant

				END
			end

			FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @QuoteStageId, @AcceptedDate, @DeclinedDate, @IsClosed, @QuoteUpdateDate
			END
		CLOSE DataCursor
		DEALLOCATE DataCursor
END

