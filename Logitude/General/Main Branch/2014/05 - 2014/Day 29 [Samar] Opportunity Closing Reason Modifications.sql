
declare @OpportunityId as varchar(15)
declare @Tenant as int
declare @ReasonCode as varchar(2)
declare @ReasonId as varchar(15)

BEGIN 
		DECLARE OpportunitiesCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, ClosingReasonCode
		FROM Opportunities
		OPEN OpportunitiesCursor FETCH NEXT FROM OpportunitiesCursor INTO @OpportunityId, @Tenant, @ReasonCode
			WHILE @@FETCH_STATUS = 0
			BEGIN

			if(@ReasonCode is not null)
			begin

				set @ReasonId = (select Id from OpportunityClosingReasons where Code = @ReasonCode and Tenant = @Tenant)

				update Opportunities 
				set ClosingReasonId = @ReasonId
				where Id = @OpportunityId and Tenant = @Tenant
			end

				FETCH NEXT FROM OpportunitiesCursor INTO @OpportunityId, @Tenant, @ReasonCode	
			END
		CLOSE OpportunitiesCursor
		DEALLOCATE OpportunitiesCursor
END

 alter table Opportunities drop constraint Opportunity_ClosingReason 
 go

 alter table Opportunities drop column ClosingReasonCode 
 go

 drop table ClosingReasons 
 go


 
