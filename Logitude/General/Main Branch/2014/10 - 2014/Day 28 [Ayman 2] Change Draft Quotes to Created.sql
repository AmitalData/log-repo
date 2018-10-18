
declare @Tenant as int
declare @QuoteId as varchar(15)
declare @QuoteStageId as varchar(15)
declare @DraftStageId as varchar(15)
declare @CreateStageId as varchar(15)


BEGIN
		DECLARE DataCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, StageId
		FROM Quotes
		OPEN DataCursor FETCH NEXT FROM DataCursor INTO @QuoteId, @Tenant, @QuoteStageId
		WHILE @@FETCH_STATUS = 0
			BEGIN		

			set @DraftStageId = (select Id from QuoteStages where Tenant = @Tenant AND Code = 'QTDR')
			set @CreateStageId = (select Id from QuoteStages where Tenant = @Tenant AND Code = 'QTCR')

			if (@QuoteStageId = @DraftStageId)
			begin
				if not exists (select * from QuoteDocumentVersions where Tenant = @Tenant AND QuoteId = @QuoteId)
				begin					
					update Quotes set StageId = @CreateStageId where Tenant = @Tenant AND Id = @QuoteId
				end
			end

			FETCH NEXT FROM DataCursor INTO @QuoteId, @Tenant, @QuoteStageId
			END
		CLOSE DataCursor
		DEALLOCATE DataCursor
END

--select * from QuoteStages
--select StageId from Quotes