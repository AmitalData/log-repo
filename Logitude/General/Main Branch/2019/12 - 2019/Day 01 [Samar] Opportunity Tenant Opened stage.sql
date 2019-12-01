

	declare @Tenant as int
	declare @OpportunityId as varchar(15)
	declare @CustomerId as varchar(15)
	declare @ExternalId as varchar(25)
	declare @TenantOpenedStageId as varchar(15)
	declare @NewId as varchar(15)

	DECLARE OpportunitiesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, CustomerId
	FROM Opportunities	
	where Tenant = 1 and CustomerId is not null
	OPEN OpportunitiesCursor FETCH NEXT FROM OpportunitiesCursor INTO @OpportunityId, @Tenant, @CustomerId
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		set @TenantOpenedStageId = (select Id from Stages where Code = 'TRS' and Tenant = @Tenant)
		set @ExternalId = (select ReceivablesAccountingCard from Cards where Id = @CustomerId and Tenant = @Tenant)

		if(@ExternalId is not null and @TenantOpenedStageId is not null)
		begin			
			if not exists(select * from OpportunityStages where OpportunityId = @OpportunityId and Tenant = @Tenant and ToStageId = @TenantOpenedStageId)
			begin
				EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'OpportunityStage'

				insert into OpportunityStages(Id, Tenant, OpportunityId, FromStageId, ToStageId, StartDate, EndDate)
				values (@NewId, @Tenant, @OpportunityId, @TenantOpenedStageId, @TenantOpenedStageId, GETDATE(), GETDATE())
			end
		end

	FETCH NEXT FROM OpportunitiesCursor INTO @OpportunityId, @Tenant, @CustomerId
	END
	CLOSE OpportunitiesCursor
	DEALLOCATE OpportunitiesCursor