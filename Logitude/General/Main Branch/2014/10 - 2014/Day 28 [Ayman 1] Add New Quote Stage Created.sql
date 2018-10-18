


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
		set @Code = 'QTCR'
		set @Name = 'Created'

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