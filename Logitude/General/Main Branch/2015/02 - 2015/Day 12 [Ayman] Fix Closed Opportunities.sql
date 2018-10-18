
declare @Tenant as int
declare @OpportunityId as varchar(15)
declare @CloseWonStageId as varchar(15)
declare @CloseLostStageId as varchar(15)

BEGIN
		DECLARE TenantsCursor CURSOR READ_ONLY
		FOR
		SELECT Id
		FROM Tenants
		OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
		WHILE @@FETCH_STATUS = 0
		BEGIN
		
			set @CloseWonStageId = (select Id from Stages where Tenant = @Tenant AND Code = 'CWN')
			set @CloseLostStageId = (select Id from Stages where Tenant = @Tenant AND Code = 'CLS')

			-- Loop Opportunities
			BEGIN
			DECLARE OpportunitiesCursor CURSOR READ_ONLY
			FOR
			SELECT Id
			FROM Opportunities
			where Tenant = @Tenant AND IsClosed = 1 AND StageId != @CloseWonStageId AND StageId != @CloseLostStageId
			OPEN OpportunitiesCursor FETCH NEXT FROM OpportunitiesCursor INTO @OpportunityId
			WHILE @@FETCH_STATUS = 0
			BEGIN

				update Opportunities
				set StageId = @CloseLostStageId
				where Id = @OpportunityId AND Tenant = @Tenant

			FETCH NEXT FROM OpportunitiesCursor INTO @OpportunityId	
			END
			CLOSE OpportunitiesCursor
			DEALLOCATE OpportunitiesCursor
			END

		FETCH NEXT FROM TenantsCursor INTO @Tenant	
		END
		CLOSE TenantsCursor
		DEALLOCATE TenantsCursor
END


