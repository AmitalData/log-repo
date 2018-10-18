
-- After migration
declare @Tenant as int
declare @NewStageId as varchar(15)
declare @Code as varchar(4)
declare @Name as varchar(40)

	DECLARE TenantsCursor CURSOR READ_ONLY
	FOR	
	SELECT Id
	FROM Tenants
	OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

		-- [1] Draft
		set @Code = 'QTDR'
		set @Name = 'Draft'
		if exists (select * from QuoteStages where Tenant = @Tenant AND Code = @Code)								
		update QuoteStages set Name = @Name, MaxDays = 0, SearchFields = @Code + ',' + @Name where Tenant = @Tenant AND Code = @Code
		
		else
		Begin
			EXECUTE usp_GetNextTableIdValue @NewStageId OUTPUT,'QuoteStage'
			insert into QuoteStages(Id, Tenant, Code, Name, MaxDays, SearchFields,UpdateDate,UpdatedByUserId,Rank)
			values(@NewStageId, @Tenant, @Code, @Name, 0, @Code + ',' + @Name,NULL,NULL,0)
		END

		-- [2] Sent
		set @Code = 'QTST'
		set @Name = 'Sent'
		if exists (select * from QuoteStages where Tenant = @Tenant AND Code = @Code)								
		update QuoteStages set Name = @Name, MaxDays = 0, SearchFields = @Code + ',' + @Name where Tenant = @Tenant AND Code = @Code
		
		else
		Begin
			EXECUTE usp_GetNextTableIdValue @NewStageId OUTPUT,'QuoteStage'
			insert into QuoteStages(Id, Tenant, Code, Name, MaxDays, SearchFields,UpdateDate,UpdatedByUserId,Rank)
			values(@NewStageId, @Tenant, @Code, @Name, 0, @Code + ',' + @Name,NULL,NULL,0)
		END

		-- [3] Viewed
		set @Code = 'QTVW'
		set @Name = 'Viewed'
		if exists (select * from QuoteStages where Tenant = @Tenant AND Code = @Code)								
		update QuoteStages set Name = @Name, MaxDays = 0, SearchFields = @Code + ',' + @Name where Tenant = @Tenant AND Code = @Code
		
		else
		Begin
			EXECUTE usp_GetNextTableIdValue @NewStageId OUTPUT,'QuoteStage'
			insert into QuoteStages(Id, Tenant, Code, Name, MaxDays, SearchFields,UpdateDate,UpdatedByUserId,Rank)
			values(@NewStageId, @Tenant, @Code, @Name, 0, @Code + ',' + @Name,NULL,NULL,0)
		END

		-- [4] In Discussion
		set @Code = 'QTID'
		set @Name = 'In Discussion'
		if exists (select * from QuoteStages where Tenant = @Tenant AND Code = @Code)								
		update QuoteStages set Name = @Name, MaxDays = 0, SearchFields = @Code + ',' + @Name where Tenant = @Tenant AND Code = @Code
		
		else
		Begin
			EXECUTE usp_GetNextTableIdValue @NewStageId OUTPUT,'QuoteStage'
			insert into QuoteStages(Id, Tenant, Code, Name, MaxDays, SearchFields,UpdateDate,UpdatedByUserId,Rank)
			values(@NewStageId, @Tenant, @Code, @Name, 0, @Code + ',' + @Name,NULL,NULL,0)
		END

		-- [5] Accepted
		set @Code = 'QTAC'
		set @Name = 'Accepted'
		if exists (select * from QuoteStages where Tenant = @Tenant AND Code = @Code)								
		update QuoteStages set Name = @Name, MaxDays = 0, SearchFields = @Code + ',' + @Name where Tenant = @Tenant AND Code = @Code
		
		else
		Begin
			EXECUTE usp_GetNextTableIdValue @NewStageId OUTPUT,'QuoteStage'
			insert into QuoteStages(Id, Tenant, Code, Name, MaxDays, SearchFields,UpdateDate,UpdatedByUserId,Rank)
			values(@NewStageId, @Tenant, @Code, @Name, 0, @Code + ',' + @Name,NULL,NULL,0)
		END

		-- [6] Declined
		set @Code = 'QTDC'
		set @Name = 'Declined'
		if exists (select * from QuoteStages where Tenant = @Tenant AND Code = @Code)								
		update QuoteStages set Name = @Name, MaxDays = 0, SearchFields = @Code + ',' + @Name where Tenant = @Tenant AND Code = @Code
		
		else
		Begin
			EXECUTE usp_GetNextTableIdValue @NewStageId OUTPUT,'QuoteStage'
			insert into QuoteStages(Id, Tenant, Code, Name, MaxDays, SearchFields,UpdateDate,UpdatedByUserId,Rank)
			values(@NewStageId, @Tenant, @Code, @Name, 0, @Code + ',' + @Name,NULL,NULL,0)
		END

	FETCH NEXT FROM TenantsCursor INTO @Tenant
	END
	CLOSE TenantsCursor
	DEALLOCATE TenantsCursor